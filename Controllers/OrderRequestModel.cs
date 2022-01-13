using System;

namespace FoodDelivery.Controllers
{
    public class OrderRequestModel
    {
        public int Id { get; set; }

        public string Date { get; set; }

        public int Status { get; set; }

        public string UserName { get; set; }

        public string RestaurateurName { get; set;}

        public string RiderName { get; set;}

        public OrderRequestModel(int id, string date, int status, string userName, string restaurateurName, string riderName)
        {
            Id = id; 
            Date = date;
            Status = status;
            UserName = userName;
            RestaurateurName = restaurateurName;
            RiderName = riderName;
        }
    }
}
