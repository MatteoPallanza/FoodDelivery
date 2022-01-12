using FoodDelivery.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace FoodDelivery.Services.Orders.Queries
{
    public class GetAllOrders : IRequest<List<Order>>
    {
        public string UserId { get; init; }

        public string UserRole { get; init; }

        public int OrderStatus { get; init; }
    }

    public class GetAllOrdersHandler : IRequestHandler<GetAllOrders, List<Order>>
    {
        readonly ApplicationDbContext _context;

        public GetAllOrdersHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<Order>> Handle(GetAllOrders request, CancellationToken cancellationToken)
        {
            if ( (request.UserId != null) && (request.UserRole != null))
            {
                var status = request.OrderStatus;
                var role = request.UserRole[0].ToString().ToUpper() + request.UserRole.Substring(1);

                if (role.Equals("Restaurateur"))
                {
                    if (request.OrderStatus >= 0)
                    {
                        var innerQueryResult =
                            (from order in _context.Orders
                             where order.RestaurateurId == request.UserId && order.Status == request.OrderStatus
                             select order).ToListAsync();

                        return innerQueryResult;
                    }

                    var queryResult =
                        (from order in _context.Orders
                         where order.RestaurateurId == request.UserId
                         select order).ToListAsync();

                    return queryResult;
                }
                else if (role.Equals("Rider"))
                {
                    if (request.OrderStatus >= 0)
                    {
                        var innerQueryResult =
                            (from order in _context.Orders
                             where order.RiderId == request.UserId && order.Status == request.OrderStatus
                             select order).ToListAsync();

                        return innerQueryResult;
                    }
                    var queryResult =
                        (from order in _context.Orders
                         where order.RiderId == request.UserId
                         select order).ToListAsync();

                    return queryResult;
                }
                else
                {
                    if (request.OrderStatus >= 0)
                    {
                        var innerQueryResult =
                            (from order in _context.Orders
                             where order.UserId == request.UserId && order.Status == request.OrderStatus
                             select order).ToListAsync();

                        return innerQueryResult;
                    }

                    var queryResult =
                        (from order in _context.Orders
                         where order.UserId == request.UserId
                         select order).ToListAsync();

                    return queryResult;
                }
            }
            else if (request.OrderStatus >= 0)
            {
                var queryResult =
                (from order in _context.Orders
                 where order.Status == request.OrderStatus
                 select order).ToListAsync();

                return queryResult;
            }
            else
            {
                return _context.Orders.ToListAsync();
            }
        }
    }
}
