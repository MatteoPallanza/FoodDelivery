using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodDelivery.Pages.Final
{
    [Authorize]
    public class OrderCompletedModel : PageModel
    {
        public int OrderId { get; set; }
        public void OnGet(int orderId)
        {
            OrderId = orderId;
        }
    }
}
