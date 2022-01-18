using FoodDelivery.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodDelivery.Pages.Admin
{
    [Authorize(Policy = PolicyName.IsAdmin)]
    public class OrdersModel : PageModel
    {
    }
}
