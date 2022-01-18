using FoodDelivery.Data;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Services.Reviews.Queries
{
    public class GetReviews : IRequest<List<GetReviewsModel>>
    {
        public string UserId { get; init; }
    }

    public class GetReviewsHandler : IRequestHandler<GetReviews, List<GetReviewsModel>>
    {
        readonly ApplicationDbContext _context;

        public GetReviewsHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<GetReviewsModel>> Handle(GetReviews request, CancellationToken cancellationToken)
        {
            if (request.UserId != null)
            {
                return
                    (from o in _context.Orders
                     where o.UserId == request.UserId && o.Status == 4
                     join restaurateur in _context.Users on o.RestaurateurId equals restaurateur.Id
                     join rider in _context.Users on o.RiderId equals rider.Id
                     join review in _context.Reviews on o.Id equals review.OrderId
                     select new GetReviewsModel(o.Id, o.Date.ToString("dd/MM/yyyy HH:mm"), restaurateur.UserName, rider.UserName, review.Rating, review.ReviewTitle, review.ReviewText)).ToListAsync();
            }
            else
            {
                return null;
            }
        }
    }
}
