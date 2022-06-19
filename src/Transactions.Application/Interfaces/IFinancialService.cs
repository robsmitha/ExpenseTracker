using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transactions.Application.Models;

namespace Transactions.Application.Interfaces
{
    public interface IFinancialService
    {
        Task<ItemModel> GetItemAsync(string accessToken);
        Task<List<AccountModel>> GetAccountsAsync(string accessToken);
        Task<string> RemoveItemAsync(string accessToken);
        Task<List<InstitutionModel>> GetInstitutionsAsync(int count, int offset);
        Task<InstitutionModel> GetInstitutionAsync(string institutionId);
        Task<List<TransactionModel>> GetTransactionsAsync(string accessToken, DateTime startDate, DateTime endDate);
        Task<string> RefreshTransactionsAsync(string accessToken);
        Task<AccessTokenModel> SetAccessTokenAsync(string accessToken);
        Task<LinkTokenModel> CreateLinkTokenAsync(string accessToken = null);
        Task<ItemPublicTokenExchangeModel> ItemPublicTokenExchangeAsync(string publicToken);
    }
}
