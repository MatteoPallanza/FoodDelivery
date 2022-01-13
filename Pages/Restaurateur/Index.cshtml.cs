using FoodDelivery.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodDelivery.Pages.Restaurateur
{
    [Authorize (Policy = PolicyName.IsRestaurateur)]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
