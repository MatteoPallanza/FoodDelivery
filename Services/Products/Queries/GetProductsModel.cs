namespace FoodDelivery.Services.Products.Queries
{
    public class GetProductsModel
    {
        public GetProductsModel(int id, string name, decimal price, decimal discount, int categoryId)
        {
            Id = id;
            Name = name;
            Price = price;
            Discount = discount;
            CategoryId = categoryId;
        }

        public int Id { get; }
        public string Name { get; }
        public decimal Price { get; }
        public decimal Discount { get; }
        public int CategoryId { get; }
    }
}
