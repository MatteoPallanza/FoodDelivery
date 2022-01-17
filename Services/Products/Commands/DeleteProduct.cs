using FoodDelivery.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace FoodDelivery.Services.Products.Commands
{
    public class DeleteProduct : AbstractProductRequest
    {
    }

    public class DeleteProductHandler : IRequestHandler<DeleteProduct, Product>
    {
        readonly ApplicationDbContext _context;

        public DeleteProductHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> Handle(DeleteProduct request, CancellationToken cancellationToken)
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

            _context.Entry(product).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return product;
        }
    }
}
