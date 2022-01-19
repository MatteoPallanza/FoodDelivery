using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodDelivery.Pages.Rider
{
    public class DeliveredModel : PageModel
    {
        public int OrderId { get; set; }
        public void OnGet(int orderId)
        {
            OrderId = orderId;
        }
    }
}
