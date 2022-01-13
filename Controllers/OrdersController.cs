using FoodDelivery.Data;
using FoodDelivery.Services.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodDelivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public Task<List<OrderRequestModel>> Get(string userName, string userRole, int status) => _mediator.Send(new GetAllOrders() { UserName = userName, UserRole = userRole, Status = status });

    }
}
