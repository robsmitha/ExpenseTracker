using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Application.Interfaces;
using Transactions.Application.Models;
using Transactions.Domain.Entities;
using Transactions.Infrastructure.Context;

namespace Transactions.Infrastructure.Services
{
    public class AccessTokenService : IAccessTokenService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        public AccessTokenService(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<AccessTokenModel>> GetAccessTokensAsync(string userId)
        {
            var accessToken = await _context.AccessTokens.Where(a => a.UserId == userId).ToListAsync();
            return _mapper.Map<List<AccessTokenModel>>(accessToken);
        }

        public async Task<AccessTokenModel> SetAccessTokenAsync(string userId, string accessToken, string itemId)
        {
            var userAccessItem = new UserAccessItem
            {
                UserId = userId,
                AccessToken = accessToken,
                ItemId = itemId
            };
            await _context.AddAsync(userAccessItem);
            await _context.SaveChangesAsync();
            return _mapper.Map<AccessTokenModel>(accessToken);
        }
    }
}
