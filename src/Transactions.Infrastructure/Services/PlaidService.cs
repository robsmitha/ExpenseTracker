
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Transactions.Application.Exceptions;
using Transactions.Application.Extensions;
using Transactions.Application.Interfaces;
using Transactions.Application.Models;
using Transactions.Domain.Responses;
using Transactions.Infrastructure.Settings;

namespace Transactions.Infrastructure.Services
{
    public class PlaidService : IFinancialService
    {
        private readonly IMapper _mapper;
        private readonly HttpClient _client;
        private readonly PlaidSettings _plaidSettings;

        private readonly ICategoryService _categoryService;
        private readonly IAccessTokenService _accessTokenService;
        private readonly ILogger<IFinancialService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PlaidService(HttpClient httpClient, IOptions<PlaidSettings> plaidSettings, IMapper mapper,
            IAccessTokenService accessTokenService, ICategoryService categoryService, ILogger<IFinancialService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _client = httpClient;
            _plaidSettings = plaidSettings.Value;
            _mapper = mapper;
            _accessTokenService = accessTokenService;
            _categoryService = categoryService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AccessTokenModel> SetAccessTokenAsync(string accessToken)
        {
            var item = await GetItemAsync(accessToken);
            if (item.HasError)
            {
                _logger.LogError($"Item [{item.ItemId}] returned error code: {item.ErrorCode}");
                throw new FinancialServiceException($"Error code: {item.ErrorCode} ({item.ItemId})");
            }
            return await _accessTokenService.SetAccessTokenAsync(accessToken, item.ItemId, item.InstitutionId);
        }

        public async Task<List<TransactionModel>> GetTransactionsAsync(string accessToken, DateTime startDate, DateTime endDate)
        {
            // get api response from plaid
            var response = await PostAsync<TransactionResponse>("transactions/get", new
            {
                client_id = _plaidSettings.ClientId,
                secret = _plaidSettings.Secret,
                access_token = accessToken,
                start_date = startDate.ToString("yyyy-MM-dd"),
                end_date = endDate.ToString("yyyy-MM-dd")
            });

            if(response?.accounts.Any() != true)
            {
                throw new FinancialServiceException($"{nameof(response.accounts)} cannot be null or empty.");
            }

            if(response?.transactions == null)
            {
                throw new FinancialServiceException($"{nameof(response.transactions)} cannot be null.");
            }

            // join accounts and transactions
            var accounts = _mapper.Map<List<AccountModel>>(response.accounts);
            var transactions = _mapper.Map<List<TransactionModel>>(response.transactions);
         
            var results = transactions.Join(
                accounts,
                t => t.account_id,
                a => a.account_id,
                (t, a) =>
                {
                    t.Account = a;
                    return t;
                }).ToList();

            return results;
        }

        public async Task<LinkTokenModel> CreateLinkTokenAsync(string accessToken = null)
        {
            var response = string.IsNullOrEmpty(accessToken)
                ? await PostAsync<CreateLinkTokenResponse>("link/token/create", new
                {
                    client_id = _plaidSettings.ClientId,
                    secret = _plaidSettings.Secret,
                    user = new
                    {
                        client_user_id = _httpContextAccessor.GetUserId()
                    },
                    client_name = "Expense Tracker",
                    products = new[] { "transactions" },
                    country_codes = new[] { "US" },
                    language = "en"
                })
                : await PostAsync<CreateLinkTokenResponse>("link/token/create", new
                {
                    client_id = _plaidSettings.ClientId,
                    secret = _plaidSettings.Secret,
                    user = new
                    {
                        client_user_id = _httpContextAccessor.GetUserId()
                    },
                    client_name = "Expense Tracker",
                    access_token = accessToken,
                    country_codes = new[] { "US" },
                    language = "en"
                });

            return _mapper.Map<LinkTokenModel>(response);
        }

        public async Task<ItemPublicTokenExchangeModel> ItemPublicTokenExchangeAsync(string publicToken)
        {
            var response = await PostAsync<ItemPublicTokenExchangeResponse>("item/public_token/exchange", new
            {
                client_id = _plaidSettings.ClientId,
                secret = _plaidSettings.Secret,
                public_token = publicToken
            });

            return _mapper.Map<ItemPublicTokenExchangeModel>(response);
        }

        public async Task<string> RefreshTransactionsAsync(string accessToken)
        {
            var response = await PostAsync<dynamic>("transactions/refresh", new
            {
                client_id = _plaidSettings.ClientId,
                access_token = accessToken,
                secret = _plaidSettings.Secret
            });
            return response.request_id as string;
        }

        public async Task<ItemModel> GetItemAsync(string accessToken)
        {
            var response = await PostAsync<ItemResponse>("item/get", new
            {
                client_id = _plaidSettings.ClientId,
                secret = _plaidSettings.Secret,
                access_token = accessToken
            });

            return _mapper.Map<ItemModel>(response);
        }

        public async Task<string> RemoveItemAsync(string accessToken)
        {
            var response = await PostAsync<dynamic>("item/remove", new
            {
                client_id = _plaidSettings.ClientId,
                secret = _plaidSettings.Secret,
                access_token = accessToken
            });

            return response.request_id as string;
        }

        public async Task<InstitutionModel> GetInstitutionAsync(string institutionId)
        {
            var response = await PostAsync<InstitutionResponse>("institutions/get_by_id", new
            {
                client_id = _plaidSettings.ClientId,
                secret = _plaidSettings.Secret,
                institution_id = institutionId,
                country_codes = new [] { "US" }
            });
            return _mapper.Map<InstitutionModel>(response.institution);
        }
        public async Task<List<InstitutionModel>> GetInstitutionsAsync(int count, int offset)
        {
            var response = await PostAsync<InstitutionResponse>("institutions/get", new
            {
                client_id = _plaidSettings.ClientId,
                secret = _plaidSettings.Secret,
                count = count,
                offset = offset,
                country_codes = new[] { "US" }
            });
            return _mapper.Map<List<InstitutionModel>>(response.institution);
        }
        public async Task<List<AccountModel>> GetAccountsAsync(string accessToken)
        {
            var response = await PostAsync<GetAccountsResponse>("accounts/get", new
            {
                client_id = _plaidSettings.ClientId,
                secret = _plaidSettings.Secret,
                access_token = accessToken
            });
            var accounts = _mapper.Map<List<AccountModel>>(response.accounts);
            return accounts;
        }

        private async Task<T> PostAsync<T>(string endpoint, dynamic data)
        {
            try
            {
                var uri = RequestUri(endpoint);
                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(uri, content);
                var result = await response.Content.ReadAsStringAsync();
                var json = JsonConvert.SerializeObject(result);
                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<Error>(result);
                    throw new FinancialServiceException(error);
                }

                return JsonConvert.DeserializeObject<T>(result);
            }
            catch (HttpRequestException)
            {
                throw;
            }
        }

        private async Task<T> SendAsync<T>(string endpoint)
        {
            try
            {
                var uri = RequestUri(endpoint);
                var response = await _client.GetAsync(uri);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<Error>(result);
                    throw new FinancialServiceException(error.error_message);
                }

                return JsonConvert.DeserializeObject<T>(result);
            }
            catch (HttpRequestException)
            {
                throw;
            }
        }

        private string RequestUri(string endpoint)
        {
            var requestUri = new StringBuilder();
            requestUri.Append(_plaidSettings.BaseUrl);
            requestUri.Append($"/{endpoint}");
            return requestUri.ToString();
        }
    }
}
