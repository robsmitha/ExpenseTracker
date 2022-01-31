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
        public string PublicToken { get; set; }
        public SetAccessTokenCommand(string userId, string publicToken)
        {
            UserId = userId;
            PublicToken = publicToken;
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
                var publicTokenExchangeResponse = await _financialService.ItemPublicTokenExchangeAsync(request.PublicToken);
                return await _financialService.SetAccessTokenAsync(request.UserId, publicTokenExchangeResponse.access_token);
            }
        }
    }
}
