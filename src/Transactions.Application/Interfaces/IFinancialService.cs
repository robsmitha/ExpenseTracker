using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transactions.Application.Models;

namespace Transactions.Application.Interfaces
{
    public interface IFinancialService
    {
        Task<ItemModel> GetItemAsync(string userId);
        Task<string> RemoveItemAsync(string userId);

        Task<List<TransactionModel>> GetTransactionsAsync(string userId, DateTime startDate, DateTime endDate);
        Task<string> RefreshTransactionsAsync(string userId);
        Task<AccessTokenModel> SetAccessTokenAsync(string userId, string token);
    }
}
