using FoodDelivery.Authorization;
using FoodDelivery.Data;
using FoodDelivery.Services.ProductCategories.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodDelivery.Pages.Restaurateur
{
    [Authorize(Policy = PolicyName.IsRestaurateur)]
    public class ProductsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        readonly IMediator _mediator;

        public ProductsModel(UserManager<ApplicationUser> userManager, IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        public string UserId { get; set; }

        public IEnumerable<ProductCategory> Categories { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            UserId = user.Id;
            Categories = await _mediator.Send(new GetAllProductCategories());

            return Page();
        }
    }
}
