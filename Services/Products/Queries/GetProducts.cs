using FoodDelivery.Data;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Services.Products.Queries
{
    public class GetProducts : IRequest<List<GetProductsModel>>
    {
        public string UserId { get; init; }
    }

    public class GetProductsHandler : IRequestHandler<GetProducts, List<GetProductsModel>>
    {
        readonly ApplicationDbContext _context;

        public GetProductsHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<GetProductsModel>> Handle(GetProducts request, CancellationToken cancellationToken)
        {
            if (request.UserId != null)
            {
                return
                    (from p in _context.Products
                     where p.UserId == request.UserId
                     select new GetProductsModel(p.Id, p.Name, p.Price, p.Discount, p.CategoryId)).ToListAsync();
            }
            else
            {
                return null;
            }
        }
    }
}
