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
    public class GetOrders : IRequest<List<GetOrdersModel>>
    {
        public string UserName { get; init; }

        public string UserRole { get; init; }

        public int Status { get; init; }
    }

    public class GetOrdersHandler : IRequestHandler<GetOrders, List<GetOrdersModel>>
    {
        readonly ApplicationDbContext _context;

        public GetOrdersHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<GetOrdersModel>> Handle(GetOrders request, CancellationToken cancellationToken)
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
                                 join rider in _context.Users on order.RiderId equals rider.Id into leftRiderJoin
                                 from element in leftRiderJoin.DefaultIfEmpty()
                                 where restaurateur.UserName == request.UserName && order.Status == request.Status
                                 orderby order.Date descending
                                 select new GetOrdersModel(order.Id, order.Date.ToString("dd/MM/yyyy HH:mm"), order.Status, user.UserName, restaurateur.UserName, element.UserName ?? string.Empty, order.DeliveryAddress)).ToListAsync();
                        case "rider":
                            return
                                (from order in _context.Orders
                                 join user in _context.Users on order.UserId equals user.Id
                                 join restaurateur in _context.Users on order.RestaurateurId equals restaurateur.Id
                                 join rider in _context.Users on order.RiderId equals rider.Id into leftRiderJoin
                                 from element in leftRiderJoin.DefaultIfEmpty()
                                 where element.UserName == request.UserName && order.Status == request.Status
                                 orderby order.Date descending
                                 select new GetOrdersModel(order.Id, order.Date.ToString("dd/MM/yyyy HH:mm"), order.Status, user.UserName, restaurateur.UserName, element.UserName ?? string.Empty, order.DeliveryAddress)).ToListAsync();
                        default:
                            return
                                (from order in _context.Orders
                                 join user in _context.Users on order.UserId equals user.Id
                                 join restaurateur in _context.Users on order.RestaurateurId equals restaurateur.Id
                                 join rider in _context.Users on order.RiderId equals rider.Id into leftRiderJoin
                                 from element in leftRiderJoin.DefaultIfEmpty()
                                 where user.UserName == request.UserName && order.Status == request.Status
                                 orderby order.Date descending
                                 select new GetOrdersModel(order.Id, order.Date.ToString("dd/MM/yyyy HH:mm"), order.Status, user.UserName, restaurateur.UserName, element.UserName ?? string.Empty, order.DeliveryAddress)).ToListAsync();
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
                                 join rider in _context.Users on order.RiderId equals rider.Id into leftRiderJoin
                                 from element in leftRiderJoin.DefaultIfEmpty()
                                 where restaurateur.UserName == request.UserName
                                 orderby order.Date descending
                                 select new GetOrdersModel(order.Id, order.Date.ToString("dd/MM/yyyy HH:mm"), order.Status, user.UserName, restaurateur.UserName, element.UserName ?? string.Empty, order.DeliveryAddress)).ToListAsync();
                        case "rider":
                            return
                                (from order in _context.Orders
                                 join user in _context.Users on order.UserId equals user.Id
                                 join restaurateur in _context.Users on order.RestaurateurId equals restaurateur.Id
                                 join rider in _context.Users on order.RiderId equals rider.Id into leftRiderJoin
                                 from element in leftRiderJoin.DefaultIfEmpty()
                                 where element.UserName == request.UserName
                                 orderby order.Date descending
                                 select new GetOrdersModel(order.Id, order.Date.ToString("dd/MM/yyyy HH:mm"), order.Status, user.UserName, restaurateur.UserName, element.UserName ?? string.Empty, order.DeliveryAddress)).ToListAsync();
                        default:
                            return
                                (from order in _context.Orders
                                 join user in _context.Users on order.UserId equals user.Id
                                 join restaurateur in _context.Users on order.RestaurateurId equals restaurateur.Id
                                 join rider in _context.Users on order.RiderId equals rider.Id into leftRiderJoin
                                 from element in leftRiderJoin.DefaultIfEmpty()
                                 where user.UserName == request.UserName
                                 orderby order.Date descending
                                 select new GetOrdersModel(order.Id, order.Date.ToString("dd/MM/yyyy HH:mm"), order.Status, user.UserName, restaurateur.UserName, element.UserName ?? string.Empty, order.DeliveryAddress)).ToListAsync();
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
                         join rider in _context.Users on order.RiderId equals rider.Id into leftRiderJoin
                         from element in leftRiderJoin.DefaultIfEmpty()
                         orderby order.Date descending
                         select new GetOrdersModel(order.Id, order.Date.ToString("dd/MM/yyyy HH:mm"), order.Status, user.UserName, restaurateur.UserName, element.UserName ?? string.Empty, order.DeliveryAddress)).ToListAsync();
                }
                else
                {
                    return
                        (from order in _context.Orders
                         join user in _context.Users on order.UserId equals user.Id
                         join restaurateur in _context.Users on order.RestaurateurId equals restaurateur.Id
                         join rider in _context.Users on order.RiderId equals rider.Id into leftRiderJoin
                         from element in leftRiderJoin.DefaultIfEmpty()
                         orderby order.Date descending
                         select new GetOrdersModel(order.Id, order.Date.ToString("dd/MM/yyyy HH:mm"), order.Status, user.UserName, restaurateur.UserName, element.UserName ?? string.Empty, order.DeliveryAddress)).ToListAsync();
                }
            }
        }
    }
}
