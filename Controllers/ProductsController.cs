using AutoMapper;
using FoodDelivery.Data;
using FoodDelivery.Services.Products.Commands;
using FoodDelivery.Services.Products.Queries;
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
    public class ProductsController : ControllerBase
    {
        readonly IMediator _mediator;
        readonly IMapper _mapper;

        public ProductsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public Task<List<GetProductsModel>> Get(string userId) => _mediator.Send(new GetProducts() { UserId = userId });

        [HttpPost]
        public async Task<ActionResult<Product>> Post(string userId, Product request)
        {
            request.UserId = userId;
            return await _mediator.Send(_mapper.Map<CreateProduct>(request));
        }

        [HttpPut]
        public async Task<ActionResult<Product>> Put(string userId, Product request)
        {
            request.UserId = userId;
            return await _mediator.Send(_mapper.Map<UpdateProduct>(request));
        }

        [HttpDelete]
        public async Task<ActionResult<Product>> Delete(string userId, Product request)
        {
            request.UserId = userId;
            return await _mediator.Send(_mapper.Map<DeleteProduct>(request));
        }

    }
}
