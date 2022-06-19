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
    public class CreateLinkTokenCommand : IRequest<LinkTokenModel>
    {
        public string AccessToken { get; set; }

        public CreateLinkTokenCommand(string accessToken)
        {
            AccessToken = accessToken;
        }

        public class Handler : IRequestHandler<CreateLinkTokenCommand, LinkTokenModel>
        {
            private readonly ILogger<CreateLinkTokenCommand> _logger;
            private readonly IFinancialService _financialService;

            public Handler(ILogger<CreateLinkTokenCommand> logger, IFinancialService financialService)
            {
                _logger = logger;
                _financialService = financialService;
            }

            public async Task<LinkTokenModel> Handle(CreateLinkTokenCommand request, CancellationToken cancellationToken)
            {
                return await _financialService.CreateLinkTokenAsync(request.AccessToken);
            }
        }
    }
}
