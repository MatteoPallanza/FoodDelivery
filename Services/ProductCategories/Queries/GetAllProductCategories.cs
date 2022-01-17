using FoodDelivery.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FoodDelivery.Services.ProductCategories.Queries
{
    public class GetAllProductCategories : IRequest<List<ProductCategory>>
    {
    }

    public class GetAllProductCategoriesHandler : IRequestHandler<GetAllProductCategories, List<ProductCategory>>
    {
        readonly ApplicationDbContext _context;

        public GetAllProductCategoriesHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<ProductCategory>> Handle(GetAllProductCategories request, CancellationToken cancellationToken)
        {
            return _context.ProductCategories.ToListAsync();
        }
    }
}
