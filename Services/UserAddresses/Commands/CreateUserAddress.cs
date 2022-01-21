using FoodDelivery.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;

namespace FoodDelivery.Services.UserAddresses.Commands
{
    public class CreateUserAddress : AbstractUserAddressRequest
    {
    }

    public class CreateUserAddressHandler : IRequestHandler<CreateUserAddress, UserAddress>
    {
        readonly ApplicationDbContext _context;
        readonly IMapper _mapper;

        public CreateUserAddressHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserAddress> Handle(CreateUserAddress request, CancellationToken cancellationToken)
        {
            var address = _mapper.Map<UserAddress>(request);
                
            _context.UserAddresses.Add(address);
            await _context.SaveChangesAsync();

            return address;
        }
    }
}
