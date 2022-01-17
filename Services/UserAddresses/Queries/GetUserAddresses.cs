using FoodDelivery.Data;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Services.UserAddresses.Queries
{
    public class GetUserAddresses : IRequest<List<GetUserAddressesModel>>
    {
        public string UserId { get; init; }
    }

    public class GetUserAddressesHandler : IRequestHandler<GetUserAddresses, List<GetUserAddressesModel>>
    {
        readonly ApplicationDbContext _context;

        public GetUserAddressesHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<GetUserAddressesModel>> Handle(GetUserAddresses request, CancellationToken cancellationToken)
        {
            if (request.UserId != null)
            {
                return 
                    (from ua in _context.UserAddresses
                     join user in _context.Users on ua.UserId equals user.Id
                     where user.Id == request.UserId
                     select new GetUserAddressesModel(ua.Id, ua.Address, ua.City, ua.PostalCode)).ToListAsync();
            }
            else
            {
                return null;
            }
        }
    }
}
