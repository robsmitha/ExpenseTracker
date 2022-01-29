
using AutoMapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Transactions.Application.Exceptions;
using Transactions.Application.Interfaces;
using Transactions.Application.Models;
using Transactions.Domain.Responses;
using Transactions.Infrastructure.Context;
using Transactions.Infrastructure.Settings;

namespace Transactions.Infrastructure.Services
{
    public class PlaidService : IFinancialService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationContext _context;
        private readonly ICategoryService _categoryService;
        private readonly HttpClient _client;
        private readonly PlaidSettings _plaidSettings;

        public PlaidService(HttpClient httpClient, IOptions<PlaidSettings> plaidSettings, IMapper mapper,
            ApplicationContext context, ICategoryService categoryService)
        {
            _client = httpClient;
            _plaidSettings = plaidSettings.Value;
            _mapper = mapper;
            _context = context;
            _categoryService = categoryService;
        }

        public async Task<List<TransactionModel>> GetTransactionsAsync(DateTime startDate, DateTime endDate)
        {
            // get api response from plaid
            var response = await PostAsync<TransactionResponse>("transactions/get", new
            {
                client_id = _plaidSettings.ClientId,
                secret = _plaidSettings.Secret,
                access_token = _plaidSettings.AccessToken,
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

            // check for existing transaction categories
            var transactionIds = results.Select(t => t.transaction_id).ToList();
            var transactionCategories = await _categoryService.GetTransactionCategoriesAsync(transactionIds);
            var map = transactionCategories.ToDictionary(t => t.TransactionId);
            results.ForEach(t =>
            {
                if(map.TryGetValue(t.transaction_id, out var transactionCategory))
                {
                    t.Category = new CategoryModel
                    {
                        Name = transactionCategory.CategoryName,
                        Id = transactionCategory.Id
                    };
                }
            });

            return results;
        }


        public async Task<string> RefreshTransactionsAsync()
        {
            var response = await PostAsync<dynamic>("transactions/refresh", new
            {
                client_id = _plaidSettings.ClientId,
                access_token = _plaidSettings.AccessToken,
                secret = _plaidSettings.Secret
            });
            return response.request_id as string;
        }

        public async Task<ItemModel> GetItemAsync()
        {
            var response = await PostAsync<ItemResponse>("item/get", new
            {
                client_id = _plaidSettings.ClientId,
                secret = _plaidSettings.Secret,
                access_token = _plaidSettings.AccessToken
            });

            return new ItemModel
            {
                LastSuccessfulUpdate = response.status.transactions.last_successful_update
            };
        }

        public async Task<string> RemoveItemAsync()
        {
            var response = await PostAsync<dynamic>("item/remove", new
            {
                client_id = _plaidSettings.ClientId,
                secret = _plaidSettings.Secret,
                access_token = _plaidSettings.AccessToken
            });

            return response.request_id as string;
        }

        private async Task<T> PostAsync<T>(string endpoint, dynamic data)
        {
            try
            {
                var uri = RequestUri(endpoint);
                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(uri, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<ErrorResponse>(result);
                    throw new FinancialServiceException(error.error_message);
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
                    var error = JsonConvert.DeserializeObject<ErrorResponse>(result);
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
