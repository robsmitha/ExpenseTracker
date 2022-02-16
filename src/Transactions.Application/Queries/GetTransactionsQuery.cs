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
        private DateTime StartDate { get; set; }
        private DateTime EndDate { get; set; }
        public GetTransactionsQuery(string userId, DateTime startDate, DateTime endDate)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException($"{nameof(userId)} cannot be null or empty.");
            }

            if (startDate > endDate)
            {
                throw new ArgumentException($"{nameof(startDate)} value \"{startDate}\" cannot be after {nameof(endDate)} value \"{endDate}\"");
            }

            UserId = userId;
            StartDate = startDate;
            EndDate = endDate;
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
                var accessTokens = await _accessTokenService.GetAccessTokensAsync(request.UserId);
                var list = new List<List<TransactionModel>>();
                foreach (var accessToken in accessTokens)
                {
                    var transactions = await _financialService.GetTransactionsAsync(request.UserId, request.StartDate, request.EndDate);
                    list.Add(transactions);
                }
                return list.SelectMany(l => l.ToList()).ToList();
            }
        }
    }
}
