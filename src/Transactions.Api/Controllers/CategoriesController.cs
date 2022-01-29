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
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(IMediator mediator, ILogger<CategoriesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
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
    }
}
