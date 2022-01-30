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
            AccessToken accessToken = null;
            try
            {
                // get existing access token if any
                accessToken = await _context.AccessTokens
                    .SingleOrDefaultAsync(t => t.UserId == userId && t.Token == token);
            }
            catch (InvalidOperationException)
            {
                // multiple records, delete all and set new access token
                var duplicates = await _context.AccessTokens
                    .Where(t => t.UserId == userId && t.Token == token)
                    .ToListAsync();
                _context.RemoveRange(duplicates);
                await _context.SaveChangesAsync();
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                // create or update access token
                if (accessToken == null)
                {
                    accessToken = new AccessToken
                    {
                        UserId = userId,
                        Token = token
                    };
                    await _context.AddAsync(accessToken);
                }
                else
                {
                    accessToken.Token = token;
                }
                await _context.SaveChangesAsync();
            }
            return _mapper.Map<AccessTokenModel>(accessToken);
        }
    }
}
