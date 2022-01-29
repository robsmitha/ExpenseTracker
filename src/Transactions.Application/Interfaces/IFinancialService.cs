using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transactions.Application.Models;

namespace Transactions.Application.Interfaces
{
    public interface IFinancialService
    {
        Task<ItemModel> GetItemAsync();
        Task<string> RemoveItemAsync();

        Task<List<TransactionModel>> GetTransactionsAsync(DateTime startDate, DateTime endDate);
        Task<string> RefreshTransactionsAsync();
    }
}
