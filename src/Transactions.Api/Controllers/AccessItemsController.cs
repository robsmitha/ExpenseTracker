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
    public class AccessItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccessItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserAccessItemModel>>> GetUserAccessItems()
        {
            return Ok(await _mediator.Send(new GetUserAccessItemsQuery()));
        }

        [HttpGet("{userAccessItemId}")]
        public async Task<ActionResult<List<UserAccessItemModel>>> GetUserAccessItem(int userAccessItemId)
        {
            return Ok(await _mediator.Send(new GetUserAccessItemQuery(userAccessItemId)));
        }

        [HttpPost("SetAccessToken")]
        public async Task<ActionResult<AccessTokenModel>> SetAccessToken(ExchangePublicTokenModel model)
        {
            return Ok(await _mediator.Send(new SetAccessTokenCommand(model.public_token)));
        }

        [HttpPost("CreateLinkToken/{accessToken?}")]
        public async Task<ActionResult<LinkTokenModel>> CreateLinkToken(string accessToken)
        {

            return Ok(await _mediator.Send(new CreateLinkTokenCommand(accessToken)));
        }
    }
}
