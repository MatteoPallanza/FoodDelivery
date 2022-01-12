using FoodDelivery.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodDelivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserAddressesController : ControllerBase
    {
        readonly ApplicationDbContext _context;
        readonly UserManager<ApplicationUser> _userManager;

        public UserAddressesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IEnumerable<UserAddress>> Get()
        {
            var userId = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault().Value;

            return await _context.UserAddresses.ToListAsync();
        }
    }
}
