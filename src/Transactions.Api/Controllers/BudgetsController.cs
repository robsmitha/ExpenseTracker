using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transactions.Application.Commands;
using Transactions.Application.Models;
using Transactions.Application.Queries;

namespace Transactions.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BudgetsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<BudgetModel>>> GetBudgets()
        {
            return Ok(await _mediator.Send(new GetBudgetsQuery()));
        }

        [HttpGet("{budgetId}")]
        public async Task<ActionResult<BudgetViewModel>> GetBudget(int budgetId)
        {
            return Ok(await _mediator.Send(new GetBudgetQuery(budgetId)));
        }

        [HttpGet("{budgetId}/Transactions")]
        public async Task<ActionResult<GetTransactionsQuery.Response>> GetBudgetTransactions(int budgetId)
        {
            return Ok(await _mediator.Send(new GetTransactionsQuery(budgetId)));
        }

        [HttpPost]
        public async Task<ActionResult<BudgetModel>> SaveBudget(BudgetModel model)
        {
            return Ok(await _mediator.Send(new SaveBudgetCommand(model)));
        }

        [HttpPost("SetTransactionCategory")]
        public async Task<ActionResult<TransactionCategoryModel>> SetTransactionCategory(TransactionCategoryModel model)
        {
            return Ok(await _mediator.Send(new SetTransactionCategoryCommand(model.TransactionId, model.CategoryId, model.BudgetId)));
        }

        [HttpPost("BulkUpdateTransactionCategory")]
        public async Task<ActionResult<List<TransactionCategoryModel>>> BulkUpdateTransactionCategory(List<TransactionCategoryModel> model)
        {
            var results = new List<TransactionCategoryModel>();
            foreach (var item in model)
            {
                var result = await _mediator.Send(new SetTransactionCategoryCommand(item.TransactionId, item.CategoryId, item.BudgetId));
                results.Add(result);
            }
            return Ok(results);
        }

        [HttpPost("UpdateBudgetCategoryEstimate")]
        public async Task<ActionResult<BudgetCategoryModel>> UpdateBudgetCategoryEstimate(BudgetCategoryModel model)
        {
            return Ok(await _mediator.Send(new UpdateBudgetCategoryCommand(model.BudgetId, model.CategoryName, model.Estimate)));
        }

        [HttpPost("SetExcludedTransaction")]
        public async Task<ActionResult<BudgetExcludedTransactionModel>> SetExcludedTransaction(BudgetExcludedTransactionModel model)
        {
            return Ok(await _mediator.Send(new SetExcludedTransactionCommand(model.TransactionId, model.BudgetId)));
        }

        [HttpPost("RestoreExcludedTransaction")]
        public async Task<ActionResult<bool>> RestoreExcludedTransaction(BudgetExcludedTransactionModel model)
        {
            return Ok(await _mediator.Send(new RestoreExcludedTransactionCommand(model.TransactionId, model.BudgetId)));
        }
    }
}
