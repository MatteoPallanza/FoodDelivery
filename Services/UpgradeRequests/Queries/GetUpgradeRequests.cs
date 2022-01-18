using FoodDelivery.Data;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Services.UpgradeRequests.Queries
{
    public class GetUpgradeRequests : IRequest<List<GetUpgradeRequestsModel>>
    {
    }

    public class GetUpgradeRequestsHandler : IRequestHandler<GetUpgradeRequests, List<GetUpgradeRequestsModel>>
    {
        readonly ApplicationDbContext _context;

        public GetUpgradeRequestsHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<GetUpgradeRequestsModel>> Handle(GetUpgradeRequests request, CancellationToken cancellationToken)
        {
            return
               (from req in _context.UpgradeRequests
                join user in _context.Users on req.UserId equals user.Id
                select new GetUpgradeRequestsModel(req.Id, user.UserName, req.Role)).ToListAsync();
        }
    }
}
