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
    public class RestoreExcludedTransactionCommand : IRequest<bool>
    {
        private string TransactionId { get; set; }
        private int BudgetId { get; set; }
        public RestoreExcludedTransactionCommand(string transactionId, int budgetId)
        {
            TransactionId = transactionId;
            BudgetId = budgetId;
        }
        public class Handler : IRequestHandler<RestoreExcludedTransactionCommand, bool>
        {
            private readonly IBudgetService _budgetService;
            public Handler(IBudgetService budgetService)
            {
                _budgetService = budgetService;
            }
            public async Task<bool> Handle(RestoreExcludedTransactionCommand request, CancellationToken cancellationToken)
            {
                return await _budgetService.RestoreExcludedTransactionAsync(request.BudgetId, request.TransactionId);
            }
        }
    }
}
