using FoodDelivery.Services.UpgradeRequests.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodDelivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UpgradeRequestsController : ControllerBase
    {
        readonly IMediator _mediator;

        public UpgradeRequestsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public Task<List<GetUpgradeRequestsModel>> Get(string userId) => _mediator.Send(new GetUpgradeRequests());
    }
}
