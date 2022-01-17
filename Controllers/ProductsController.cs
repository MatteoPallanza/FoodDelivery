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

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public Task<List<GetProductsModel>> Get(string userId) => _mediator.Send(new GetProducts() { UserId = userId });

        [HttpPost]
        public async Task<ActionResult<Product>> Post(string userId, Product request)
        {
            return await _mediator.Send(new CreateProduct()
            {
                Name = request.Name,
                Price = request.Price,
                Discount = request.Discount,
                CategoryId = request.CategoryId,
                UserId = userId
            });
        }

        [HttpPut]
        public async Task<ActionResult<Product>> Put(string userId, Product request)
        {
            return await _mediator.Send(new UpdateProduct()
            {
                Id = request.Id,
                Name = request.Name,
                Price = request.Price,
                Discount = request.Discount,
                CategoryId = request.CategoryId,
                UserId = userId
            });
        }

        [HttpDelete]
        public async Task<ActionResult<Product>> Delete(string userId, Product request)
        {
            return await _mediator.Send(new DeleteProduct()
            {
                Id = request.Id,
                Name = request.Name,
                Price = request.Price,
                Discount = request.Discount,
                CategoryId = request.CategoryId,
                UserId = userId
            });
        }

    }
}
