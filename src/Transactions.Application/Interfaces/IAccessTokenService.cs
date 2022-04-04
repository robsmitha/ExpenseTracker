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
        Task<List<AccessTokenModel>> GetAccessTokensAsync(string userId);
        Task<AccessTokenModel> GetAccessTokenAsync(string userId, string itemId);
        Task<AccessTokenModel> SetAccessTokenAsync(string userId, string accessToken, string itemId);
    }
}
