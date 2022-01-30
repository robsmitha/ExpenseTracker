using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Transactions.Api.Controllers
{
    public abstract class BaseApiController<T> : ControllerBase
    {
        protected readonly ILogger<T> _logger;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public BaseApiController(ILogger<T> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId => _httpContextAccessor?
            .HttpContext
            .User
            .Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
    }
}
