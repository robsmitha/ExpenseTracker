using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Transactions.Application.Interfaces;
using Transactions.Application.Models;

namespace Transactions.Application.Commands
{
    public class LoadTransactionsCommand : IRequest<LoadTransactionsCommand.Response>
    {
        private string UserId { get; set; }
        private int BudgetId { get; set; }
        public LoadTransactionsCommand(string userId, int budgetId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException($"{nameof(userId)} cannot be null or empty.");
            }

            UserId = userId;
            BudgetId = budgetId;
        }

        public class Handler : IRequestHandler<LoadTransactionsCommand, Response>
        {
            private readonly ILogger<LoadTransactionsCommand> _logger;
            private readonly IFinancialService _financialService;
            private readonly IBudgetService _budgetService;
            private readonly IExcelService _excelService;

            public Handler(ILogger<LoadTransactionsCommand> logger, IFinancialService financialService, IExcelService excelService,
                IBudgetService budgetService)
            {
                _logger = logger;
                _financialService = financialService;
                _budgetService = budgetService;
                _excelService = excelService;
            }

            public async Task<Response> Handle(LoadTransactionsCommand request, CancellationToken cancellationToken)
            {
                var budget = await _budgetService.GetBudgetAsync(request.BudgetId);
                var transactions = await _financialService.GetTransactionsAsync(request.UserId, budget.StartDate, budget.EndDate);
                await _excelService.SaveExcelFileAsync(transactions, budget.Name);
                return new Response(transactions);
            }
        }

        public class Response
        {
            public List<TransactionModel> Transactions { get; set; }
            public Response(List<TransactionModel> transactions = null)
            {
                Transactions = transactions ?? new List<TransactionModel>();
            }
        }
    }
}
