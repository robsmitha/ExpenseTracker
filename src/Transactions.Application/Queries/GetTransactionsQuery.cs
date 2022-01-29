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
        private DateTime StartDate { get; set; }
        private DateTime EndDate { get; set; }
        public GetTransactionsQuery(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException($"{nameof(startDate)} value \"{startDate}\" cannot be after {nameof(endDate)} value \"{endDate}\"");
            }

            StartDate = startDate;
            EndDate = endDate;
        }

        public class Handler : IRequestHandler<GetTransactionsQuery, List<TransactionModel>>
        {
            private readonly IFinancialService _financialService;
            private readonly ICategoryService _categoryService;

            public Handler(IFinancialService financialService, ICategoryService categoryService)
            {
                _financialService = financialService;
                _categoryService = categoryService;
            }

            public async Task<List<TransactionModel>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
            {
                return await _financialService.GetTransactionsAsync(request.StartDate, request.EndDate);
            }
        }
    }
}
