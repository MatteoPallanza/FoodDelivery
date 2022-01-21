using AutoMapper;
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
        readonly IMapper _mapper;

        public DeleteProductHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Product> Handle(DeleteProduct request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(request);

            _context.Entry(product).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return product;
        }
    }
}
