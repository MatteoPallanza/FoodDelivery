using FoodDelivery.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace FoodDelivery.Services.Products.Commands
{
    public class UpdateProduct : AbstractProductRequest
    {
    }

    public class UpdateProductHandler : IRequestHandler<UpdateProduct, Product>
    {
        readonly ApplicationDbContext _context;

        public UpdateProductHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> Handle(UpdateProduct request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Id = request.Id,
                Name = request.Name,
                Price = request.Price,
                Discount = request.Discount,
                CategoryId = request.CategoryId,
                UserId = request.UserId
            };

            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return product;
        }
    }
}
