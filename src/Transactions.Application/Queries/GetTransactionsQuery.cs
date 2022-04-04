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

namespace Transactions.Application.Queries
{
    public class GetTransactionsQuery : IRequest<List<TransactionModel>>
    {
        private string UserId { get; set; }
        private string ItemId { get; set; }
        private DateTime StartDate { get; set; }
        private DateTime EndDate { get; set; }
        public GetTransactionsQuery(string userId, string itemId,
            DateTime? startDate = null, DateTime? endDate = null)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException($"{nameof(userId)} cannot be null or empty.");
            }

            if (startDate > endDate)
            {
                throw new ArgumentException($"{nameof(startDate)} value \"{startDate}\" cannot be after {nameof(endDate)} value \"{endDate}\"");
            }

            var now = DateTime.Now;
            startDate ??= new DateTime(now.Year, now.Month, 1);
            endDate ??= startDate.Value.AddMonths(1).AddDays(-1);


            UserId = userId;
            ItemId = itemId;
            StartDate = startDate.Value;
            EndDate = endDate.Value;
        }

        public class Handler : IRequestHandler<GetTransactionsQuery, List<TransactionModel>>
        {
            private readonly IFinancialService _financialService;
            private readonly IAccessTokenService _accessTokenService;

            public Handler(IFinancialService financialService, IAccessTokenService accessTokenService)
            {
                _financialService = financialService;
                _accessTokenService = accessTokenService;
            }

            public async Task<List<TransactionModel>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
            {
                var accessToken = await _accessTokenService.GetAccessTokenAsync(request.UserId, request.ItemId);
                if(accessToken != null)
                {
                    return await _financialService.GetTransactionsAsync(accessToken.AccessToken, request.StartDate, request.EndDate);
                }
                throw new Exception(); // MissingAccessTokenException
            }
        }
    }
}
