using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TravelExpertsData;
using TravelExpertsData.Data;

namespace TravelExpertsGui.Controllers
{
    public class AccountController : Controller
    {
        private readonly TravelExpertsContext _context; // database context 

        public AccountController(TravelExpertsContext context)
        {
            _context = context;
        }
        public IActionResult Login(string returnUrl = "")
        {
            if(returnUrl!= null) 
            {
                TempData["ReturnUrl"] = returnUrl;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(Customer customer) // data collected on the form
        {

            Customer cus = CustomerManager.Authenticate(customer.Username, customer.Password, _context); // Authenticate user
            if (cus == null) // failed authentication
            {
                TempData["Message"] = "Incorrect login details.";
                TempData["IsError"] = true;
                return View(); // stay on the login page
            }
            // usr != null   - authentication passed

            // if the user is an owner, add to session state

            string customerId = cus.CustomerId.ToString();
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, cus.Username),
                new Claim(ClaimTypes.NameIdentifier, customerId)
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme); // use cookies authentication
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal); // generates authentication cookie
            // if no return URL, go to the home page
            if (string.IsNullOrEmpty(TempData["ReturnUrl"].ToString()))
            {
                return RedirectToAction("BookPackage", "Packages");
            }
            else
            {
                return Redirect(TempData["ReturnUrl"].ToString());
            }
        }



        //Authorization
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
