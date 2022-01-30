using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Transactions.Application.Interfaces;
using Transactions.Application.Models;

namespace Transactions.Application.Commands
{
    public class SetAccessTokenCommand : IRequest<AccessTokenModel>
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public SetAccessTokenCommand(string userId, string token)
        {
            UserId = userId;
            Token = token;
        }

        public class Handler : IRequestHandler<SetAccessTokenCommand, AccessTokenModel>
        {
            private readonly IFinancialService _financialService;
            public Handler(IFinancialService financialService)
            {
                _financialService = financialService;
            }
            public async Task<AccessTokenModel> Handle(SetAccessTokenCommand request, CancellationToken cancellationToken)
            {
                return await _financialService.SetAccessTokenAsync(request.UserId, request.Token);
            }
        }
    }
}
