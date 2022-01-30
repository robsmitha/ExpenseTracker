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

            public Handler(IFinancialService financialService)
            {
                _financialService = financialService;
            }

            public async Task<List<TransactionModel>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
            {
                return await _financialService.GetTransactionsAsync(request.UserId, request.StartDate, request.EndDate);
            }
        }
    }
}
