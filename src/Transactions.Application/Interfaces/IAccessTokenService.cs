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
        Task<AccessTokenModel> GetAccessTokenAsync(string userId);
        Task<AccessTokenModel> SetAccessTokenAsync(string userId, string token);
    }
}
