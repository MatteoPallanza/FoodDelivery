using AutoMapper;
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
        readonly IMapper _mapper;

        public UpdateProductHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Product> Handle(UpdateProduct request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(request);

            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return product;
        }
    }
}
