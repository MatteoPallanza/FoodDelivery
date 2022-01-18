using System;

namespace FoodDelivery.Services.Reviews.Queries
{
    public class GetReviewsModel
    {
        public GetReviewsModel(int orderId, string date, string restaurateurName, string riderName, int rating, string reviewTitle, string reviewText)
        {
            OrderId = orderId;
            Date = date;
            RestaurateurName = restaurateurName;
            RiderName = riderName;
            Rating = rating;
            ReviewTitle = reviewTitle;
            ReviewText = reviewText;
        }

        public int OrderId { get; }
        public string Date { get; }
        public string RestaurateurName { get; }
        public string RiderName { get; }
        public int Rating { get; }
        public string ReviewTitle { get; }
        public string ReviewText { get; }
    }
}
