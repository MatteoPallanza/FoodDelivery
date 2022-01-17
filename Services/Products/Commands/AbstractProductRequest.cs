using FoodDelivery.Data;
using MediatR;

namespace FoodDelivery.Services.Products.Commands
{
    public class AbstractProductRequest : IRequest<Product>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal Discount { get; set; }

        public int CategoryId { get; set; }

        public string UserId { get; set; }
    }
}
