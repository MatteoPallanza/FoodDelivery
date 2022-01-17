using FoodDelivery.Data;
using FoodDelivery.Services.UserAddresses.Commands;
using FoodDelivery.Services.UserAddresses.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodDelivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserAddressesController : ControllerBase
    {
        readonly IMediator _mediator;

        public UserAddressesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public Task<List<GetUserAddressesModel>> Get(string userId) => _mediator.Send(new GetUserAddresses() { UserId = userId });

        [HttpPost]
        public async Task<ActionResult<UserAddress>> Post(string userId, UserAddress request)
        {
            return await _mediator.Send(new CreateUserAddress()
            {
                Address = request.Address,
                City = request.City,
                PostalCode = request.PostalCode,
                UserId = userId
            });
        }

        [HttpPut]
        public async Task<ActionResult<UserAddress>> Put(string userId, UserAddress request)
        {
            return await _mediator.Send(new UpdateUserAddress()
            {
                Id = request.Id,
                Address = request.Address,
                City = request.City,
                PostalCode = request.PostalCode,
                UserId = userId
            });
        }

        [HttpDelete]
        public async Task<ActionResult<UserAddress>> Delete(string userId, UserAddress request)
        {
            return await _mediator.Send(new DeleteUserAddress()
            {
                Id = request.Id,
                Address = request.Address,
                City = request.City,
                PostalCode = request.PostalCode,
                UserId = userId
            });
        }
    }
}
