using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Application.Commands;
using Transactions.Job.Interfaces;

namespace Transactions.Job.Services
{
    public class RuntimeService : IRuntimeService
    {
        private readonly ILogger<IRuntimeService> _logger;
        private readonly IMediator _mediator;
        public RuntimeService(ILogger<IRuntimeService> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task StartAsync()
        {
            var now = DateTime.Now;
            var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var response = await _mediator.Send(new LoadTransactionsCommand(firstDayOfMonth, lastDayOfMonth));
            _logger.LogInformation($"Loaded {response.Transactions.Count} transaction{(response.Transactions.Count == 1 ? "" : "s")}");
        }

        public void Stop()
        {

        }
    }
}
