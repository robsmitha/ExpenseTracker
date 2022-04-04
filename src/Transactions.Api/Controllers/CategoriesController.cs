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
    public class CategoriesController : BaseApiController<CategoriesController>
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator, ILogger<CategoriesController> logger, IHttpContextAccessor httpContextAccessor)
            : base(logger, httpContextAccessor)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryModel>>> GetCategoriesAsync()
        {
            return Ok(await _mediator.Send(new GetCategoriesQuery()));
        }

        [HttpPost("SetTransactionCategory")]
        public async Task<ActionResult<TransactionCategoryModel>> SetTransactionCategory(TransactionCategoryModel model)
        {
            return Ok(await _mediator.Send(new SetTransactionCategoryCommand(model.TransactionId, model.CategoryId)));
        }

        [HttpPost("BulkUpdateTransactionCategory")]
        public async Task<ActionResult<List<TransactionCategoryModel>>> BulkUpdateTransactionCategory(List<TransactionCategoryModel> model)
        {
            var results = new List<TransactionCategoryModel>();
            foreach (var item in model)
            {
                var result = await _mediator.Send(new SetTransactionCategoryCommand(item.TransactionId, item.CategoryId));
                results.Add(result);
            }
            return Ok(results);
        }
    }
}
