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
        private DateTime StartDate { get; set; }
        private DateTime EndDate { get; set; }
        public LoadTransactionsCommand(string userId, DateTime startDate, DateTime endDate)
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

        public class Handler : IRequestHandler<LoadTransactionsCommand, Response>
        {
            private readonly ILogger<LoadTransactionsCommand> _logger;
            private readonly IFinancialService _financialService;
            private readonly IExcelService _excelService;

            public Handler(ILogger<LoadTransactionsCommand> logger, IFinancialService financialService, IExcelService excelService)
            {
                _logger = logger;
                _financialService = financialService;
                _excelService = excelService;
            }

            public async Task<Response> Handle(LoadTransactionsCommand request, CancellationToken cancellationToken)
            {
                var transactions = await _financialService.GetTransactionsAsync(request.UserId, request.StartDate, request.EndDate);
                
                var worksheetName = $"{request.StartDate:Y}-{transactions.First().Account.official_name}";
                await _excelService.SaveExcelFileAsync(transactions, worksheetName);
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
