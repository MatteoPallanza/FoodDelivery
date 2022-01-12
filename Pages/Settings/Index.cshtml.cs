using FoodDelivery.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodDelivery.Pages.Settings
{
    [Authorize(Policy = PolicyName.IsAdmin)]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
