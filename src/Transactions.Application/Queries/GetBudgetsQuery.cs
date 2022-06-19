using MediatR;
using Microsoft.AspNetCore.Http;
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
    public class GetBudgetsQuery : IRequest<List<BudgetModel>>
    {
        public class Handler : IRequestHandler<GetBudgetsQuery, List<BudgetModel>>
        {
            private readonly IBudgetService _budgetService;
            public Handler(IBudgetService budgetService)
            {
                _budgetService = budgetService;
            }
            public async Task<List<BudgetModel>> Handle(GetBudgetsQuery request, CancellationToken cancellationToken)
            {
                return await _budgetService.GetBudgetsAsync();
            }
        }
    }
}
