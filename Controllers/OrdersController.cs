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
        public Task<List<Order>> Get(string userId, string userRole, int orderStatus = -1) => _mediator.Send(new GetAllOrders() { UserId = userId, UserRole = userRole, OrderStatus = orderStatus });

    }
}
