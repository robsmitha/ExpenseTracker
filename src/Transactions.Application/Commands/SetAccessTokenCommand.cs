using MediatR;
using Microsoft.Extensions.Logging;
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
            private readonly ILogger<SetAccessTokenCommand> _logger;
            private readonly IFinancialService _financialService;
            public Handler(IFinancialService financialService, ILogger<SetAccessTokenCommand> logger)
            {
                _financialService = financialService;
                _logger = logger;
            }
            public async Task<AccessTokenModel> Handle(SetAccessTokenCommand request, CancellationToken cancellationToken)
            {
                var publicTokenExchangeResponse = await _financialService.ItemPublicTokenExchangeAsync(request.PublicToken);
                
                var item = await _financialService.GetItemAsync(publicTokenExchangeResponse.access_token);
                if (item.HasError)
                {
                    _logger.LogError($"Item [{item.ItemId}] returned error code: {item.ErrorCode}");
                }
                return await _financialService.SetAccessTokenAsync(request.UserId, publicTokenExchangeResponse.access_token, item.ItemId);
            }
        }
    }
}
