using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Application.Extensions;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccessTokenService(ApplicationContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<AccessTokenModel>> GetAccessTokensAsync()
        {
            var accessToken = await _context.UserAccessItems.Where(a => a.UserId == _httpContextAccessor.GetUserId()).ToListAsync();
            return _mapper.Map<List<AccessTokenModel>>(accessToken);
        }

        public async Task<List<AccessTokenModel>> GetBudgetAccessTokensAsync(int budgetId)
        {
            var budgetAccessItemIds = await _context.BudgetAccessItems.Where(a => a.BudgetId == budgetId).Select(a => a.UserAccessItemId).ToListAsync();
            var accessToken = await _context.UserAccessItems.Where(a => budgetAccessItemIds.Contains(a.Id)).ToListAsync();
            return _mapper.Map<List<AccessTokenModel>>(accessToken);
        }

        public async Task<AccessTokenModel> GetAccessTokenAsync(string itemId)
        {
            var accessToken = await _context.UserAccessItems.FirstOrDefaultAsync(a => a.UserId == _httpContextAccessor.GetUserId() && a.ItemId == itemId);
            return _mapper.Map<AccessTokenModel>(accessToken);
        }

        public async Task<AccessTokenModel> SetAccessTokenAsync(string accessToken, string itemId, string institutionId)
        {
            var userAccessItem = new UserAccessItem
            {
                UserId = _httpContextAccessor.GetUserId(),
                AccessToken = accessToken,
                ItemId = itemId,
                InstitutionId = institutionId
            };
            await _context.AddAsync(userAccessItem);
            await _context.SaveChangesAsync();
            return _mapper.Map<AccessTokenModel>(userAccessItem);
        }
    }
}
