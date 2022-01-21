using AutoMapper;
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
        readonly IMapper _mapper;

        public CreateProductHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Product> Handle(CreateProduct request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(request);

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }
    }
}
