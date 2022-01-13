using FoodDelivery.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using FoodDelivery.Controllers;

namespace FoodDelivery.Services.Orders.Queries
{
    public class GetAllOrders : IRequest<List<OrderRequestModel>>
    {
        public string UserName { get; init; }

        public string UserRole { get; init; }

        public int Status { get; init; }
    }

    public class GetAllOrdersHandler : IRequestHandler<GetAllOrders, List<OrderRequestModel>>
    {
        readonly ApplicationDbContext _context;

        public GetAllOrdersHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<OrderRequestModel>> Handle(GetAllOrders request, CancellationToken cancellationToken)
        {
            if (request.UserName != null)
            {
                var userRole = request.UserRole ?? "final";

                if (request.Status > 0)
                {
                    switch (userRole)
                    {
                        case "restaurateur":
                            return
                                (from order in _context.Orders
                                 join user in _context.Users on order.UserId equals user.Id
                                 join restaurateur in _context.Users on order.RestaurateurId equals restaurateur.Id
                                 join rider in _context.Users on order.RiderId equals rider.Id
                                 where restaurateur.UserName == request.UserName && order.Status == request.Status
                                 select new OrderRequestModel(order.Id, order.Date.ToString("dd/MM/yyyy HH:mm"), order.Status, user.UserName, restaurateur.UserName, rider.UserName)).ToListAsync();
                        case "rider":
                            return
                                (from order in _context.Orders
                                 join user in _context.Users on order.UserId equals user.Id
                                 join restaurateur in _context.Users on order.RestaurateurId equals restaurateur.Id
                                 join rider in _context.Users on order.RiderId equals rider.Id
                                 where rider.UserName == request.UserName && order.Status == request.Status
                                 select new OrderRequestModel(order.Id, order.Date.ToString("dd/MM/yyyy HH:mm"), order.Status, user.UserName, restaurateur.UserName, rider.UserName)).ToListAsync();
                        default:
                            return
                                (from order in _context.Orders
                                 join user in _context.Users on order.UserId equals user.Id
                                 join restaurateur in _context.Users on order.RestaurateurId equals restaurateur.Id
                                 join rider in _context.Users on order.RiderId equals rider.Id
                                 where user.UserName == request.UserName && order.Status == request.Status
                                 select new OrderRequestModel(order.Id, order.Date.ToString("dd/MM/yyyy HH:mm"), order.Status, user.UserName, restaurateur.UserName, rider.UserName)).ToListAsync();
                    }
                }
                else
                {
                    switch (userRole)
                    {
                        case "restaurateur":
                            return
                                (from order in _context.Orders
                                 join user in _context.Users on order.UserId equals user.Id
                                 join restaurateur in _context.Users on order.RestaurateurId equals restaurateur.Id
                                 join rider in _context.Users on order.RiderId equals rider.Id
                                 where restaurateur.UserName == request.UserName
                                 select new OrderRequestModel(order.Id, order.Date.ToString("dd/MM/yyyy HH:mm"), order.Status, user.UserName, restaurateur.UserName, rider.UserName)).ToListAsync();
                        case "rider":
                            return
                                (from order in _context.Orders
                                 join user in _context.Users on order.UserId equals user.Id
                                 join restaurateur in _context.Users on order.RestaurateurId equals restaurateur.Id
                                 join rider in _context.Users on order.RiderId equals rider.Id
                                 where rider.UserName == request.UserName
                                 select new OrderRequestModel(order.Id, order.Date.ToString("dd/MM/yyyy HH:mm"), order.Status, user.UserName, restaurateur.UserName, rider.UserName)).ToListAsync();
                        default:
                            return
                                (from order in _context.Orders
                                 join user in _context.Users on order.UserId equals user.Id
                                 join restaurateur in _context.Users on order.RestaurateurId equals restaurateur.Id
                                 join rider in _context.Users on order.RiderId equals rider.Id
                                 where user.UserName == request.UserName
                                 select new OrderRequestModel(order.Id, order.Date.ToString("dd/MM/yyyy HH:mm"), order.Status, user.UserName, restaurateur.UserName, rider.UserName)).ToListAsync();
                    }
                }
            }
            else
            {
                if (request.Status > 0)
                {
                    return
                        (from order in _context.Orders
                         where order.Status == request.Status
                         join user in _context.Users on order.UserId equals user.Id
                         join restaurateur in _context.Users on order.RestaurateurId equals restaurateur.Id
                         join rider in _context.Users on order.RiderId equals rider.Id
                         select new OrderRequestModel(order.Id, order.Date.ToString("dd/MM/yyyy HH:mm"), order.Status, user.UserName, restaurateur.UserName, rider.UserName)).ToListAsync();
                }
                else
                {
                    return
                        (from order in _context.Orders
                         join user in _context.Users on order.UserId equals user.Id
                         join restaurateur in _context.Users on order.RestaurateurId equals restaurateur.Id
                         join rider in _context.Users on order.RiderId equals rider.Id
                         select new OrderRequestModel(order.Id, order.Date.ToString("dd/MM/yyyy HH:mm"), order.Status, user.UserName, restaurateur.UserName, rider.UserName)).ToListAsync();
                }
            }
        }
    }
}
