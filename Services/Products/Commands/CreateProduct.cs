using FoodDelivery.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FoodDelivery.Services.Products.Commands
{
    public class CreateProduct : AbstractProductRequest
    {
    }

    public class CreateProductHandler : IRequestHandler<CreateProduct, Product>
    {
        readonly ApplicationDbContext _context;

        public CreateProductHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> Handle(CreateProduct request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name,
                Price = request.Price,
                Discount = request.Discount,
                CategoryId = request.CategoryId,
                UserId = request.UserId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }
    }
}
