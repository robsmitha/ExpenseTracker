using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Application.Models;

namespace Transactions.Application.Interfaces
{
    public interface IAccessTokenService
    {
        Task<List<AccessTokenModel>> GetAccessTokensAsync();
        Task<AccessTokenModel> GetAccessTokenAsync(string itemId);
        Task<AccessTokenModel> GetAccessTokenAsync(int userAccessTokenId);
        Task<AccessTokenModel> SetAccessTokenAsync(string accessToken, string itemId, string institutionId);
        Task<List<AccessTokenModel>> GetBudgetAccessTokensAsync(int budgetId);
    }
}
