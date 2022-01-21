using AutoMapper;
using FoodDelivery.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace FoodDelivery.Services.UserAddresses.Commands
{
    public class UpdateUserAddress : AbstractUserAddressRequest
    {
    }

    public class UpdateUserAddressHandler : IRequestHandler<UpdateUserAddress, UserAddress>
    {
        readonly ApplicationDbContext _context;
        readonly IMapper _mapper;

        public UpdateUserAddressHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserAddress> Handle(UpdateUserAddress request, CancellationToken cancellationToken)
        {
            var address = _mapper.Map<UserAddress>(request);

            _context.Entry(address).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return address;
        }
    }
}
