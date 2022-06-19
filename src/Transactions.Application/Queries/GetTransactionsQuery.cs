using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Transactions.Application.Constants;
using Transactions.Application.Exceptions;
using Transactions.Application.Interfaces;
using Transactions.Application.Models;

namespace Transactions.Application.Queries
{
    public class GetTransactionsQuery : IRequest<GetTransactionsQuery.Response>
    {
        private string ItemId { get; set; }
        private DateTime StartDate { get; set; }
        private DateTime EndDate { get; set; }
        public GetTransactionsQuery(string itemId, DateTime? startDate = null, DateTime? endDate = null)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException($"{nameof(startDate)} value \"{startDate}\" cannot be after {nameof(endDate)} value \"{endDate}\"");
            }

            var now = DateTime.Now;
            startDate ??= new DateTime(now.Year, now.Month, 1);
            endDate ??= startDate.Value.AddMonths(1).AddDays(-1);


            ItemId = itemId;
            StartDate = startDate.Value;
            EndDate = endDate.Value;
        }

        public class Handler : IRequestHandler<GetTransactionsQuery, Response>
        {
            private readonly IFinancialService _financialService;
            private readonly IAccessTokenService _accessTokenService;

            public Handler(IFinancialService financialService, IAccessTokenService accessTokenService)
            {
                _financialService = financialService;
                _accessTokenService = accessTokenService;
            }

            public async Task<Response> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
            {
                var accessToken = await _accessTokenService.GetAccessTokenAsync(request.ItemId);
                var transactions = new List<TransactionModel>();
                try
                {
                    if (accessToken == null)
                    {
                        throw new NotFoundException("AccessToken not found.");
                    }
                    transactions = await _financialService.GetTransactionsAsync(accessToken.AccessToken, request.StartDate, request.EndDate);
                }
                catch (FinancialServiceException fex)
                {
                    if (string.Equals(fex.Error?.error_code, ErrorCodes.ITEM_LOGIN_REQUIRED,
                               StringComparison.InvariantCultureIgnoreCase))
                    {
                        var institution = await _financialService.GetInstitutionAsync(accessToken.InstitutionId);
                        return new Response
                        {
                            ExpiredAccessItem = new ExpiredAccessItem(accessToken.AccessToken, fex.Error.error_message, institution)
                        };
                    }
                }
                return new Response
                {
                    Transactions = transactions
                };
            }
        }
        public class Response
        {
            public List<TransactionModel> Transactions { get; set; }
            public ExpiredAccessItem ExpiredAccessItem { get; set; }
        }
    }
}
