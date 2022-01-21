using AutoMapper;
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
        readonly IMapper _mapper;

        public UserAddressesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public Task<List<GetUserAddressesModel>> Get(string userId) => _mediator.Send(new GetUserAddresses() { UserId = userId });

        [HttpPost]
        public async Task<ActionResult<UserAddress>> Post(string userId, UserAddress request)
        {
            request.UserId = userId;
            return await _mediator.Send(_mapper.Map<CreateUserAddress>(request));
        }

        [HttpPut]
        public async Task<ActionResult<UserAddress>> Put(string userId, UserAddress request)
        {
            request.UserId = userId;
            return await _mediator.Send(_mapper.Map<UpdateUserAddress>(request));
        }

        [HttpDelete]
        public async Task<ActionResult<UserAddress>> Delete(string userId, UserAddress request)
        {
            request.UserId = userId;
            return await _mediator.Send(_mapper.Map<DeleteUserAddress>(request));
        }
    }
}
