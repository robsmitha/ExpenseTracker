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

        public async Task<AccessTokenModel> GetAccessTokenAsync(string userId)
        {
            var accessToken = await _context.AccessTokens.FirstOrDefaultAsync(a => a.UserId == userId);
            return _mapper.Map<AccessTokenModel>(accessToken);
        }

        public async Task<AccessTokenModel> SetAccessTokenAsync(string userId, string token)
        {
            var accessToken = new AccessToken
            {
                UserId = userId,
                Token = token
            };
            await _context.AddAsync(accessToken);
            await _context.SaveChangesAsync();
            return _mapper.Map<AccessTokenModel>(accessToken);
        }
    }
}
