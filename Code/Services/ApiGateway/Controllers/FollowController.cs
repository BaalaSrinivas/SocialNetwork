using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api/agg/[controller]")]
    public class FollowController : ControllerBase
    {
        private readonly ILogger<FollowController> _logger;

        public FollowController(ILogger<FollowController> logger)
        {
            _logger = logger;
        }
    }
}
