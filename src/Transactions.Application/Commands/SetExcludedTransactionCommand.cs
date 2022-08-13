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
    public class SetExcludedTransactionCommand : IRequest<BudgetExcludedTransactionModel>
    {
        private string TransactionId { get; set; }
        private int BudgetId { get; set; }
        public SetExcludedTransactionCommand(string transactionId, int budgetId)
        {
            TransactionId = transactionId;
            BudgetId = budgetId;
        }
        public class Handler : IRequestHandler<SetExcludedTransactionCommand, BudgetExcludedTransactionModel>
        {
            private readonly IBudgetService _budgetService;
            public Handler(IBudgetService budgetService)
            {
                _budgetService = budgetService;
            }
            public async Task<BudgetExcludedTransactionModel> Handle(SetExcludedTransactionCommand request, CancellationToken cancellationToken)
            {
                return await _budgetService.SetExcludedTransactionAsync(request.BudgetId, request.TransactionId);
            }
        }
    }
}
