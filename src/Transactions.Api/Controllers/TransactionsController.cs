using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transactions.Application.Commands;
using Transactions.Application.Models;
using Transactions.Application.Queries;

namespace Transactions.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : BaseApiController<TransactionsController>
    {
        private readonly IMediator _mediator;

        public TransactionsController(IMediator mediator, ILogger<TransactionsController> logger, IHttpContextAccessor httpContextAccessor)
            : base(logger, httpContextAccessor)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<TransactionModel>>> GetTransactions()
        {
            var now = DateTime.Now;
            var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            return Ok(await _mediator.Send(new GetTransactionsQuery(UserId, firstDayOfMonth, lastDayOfMonth)));
        }

        [HttpGet("GetUserAccessItems")]
        public async Task<ActionResult<UserAccessItemModel>> GetUserAccessItems()
        {
            return Ok(await _mediator.Send(new GetUserAccessItemsQuery(UserId)));
        }

        [HttpPost("SetAccessToken")]
        public async Task<ActionResult<AccessTokenModel>> SetAccessToken(ExchangePublicTokenModel model)
        {
            return Ok(await _mediator.Send(new SetAccessTokenCommand(UserId, model.public_token)));
        }

        [HttpPost("CreateLinkToken")]
        public async Task<ActionResult<LinkTokenModel>> CreateLinkToken()
        {

            return Ok(await _mediator.Send(new CreateLinkTokenCommand(UserId)));
        }
    }
}
