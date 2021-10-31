using System.Linq;
using System.Threading.Tasks;
using IctFinalProject.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ictFinalProject.WebAdmin.Controllers
{
    public class AddProductController : Controller
    {
        private readonly HttpService _httpService;
        private readonly CookieAuthenticationService _authenticationService;

        public AddProductController(HttpService httpService, 
            CookieAuthenticationService authenticationService)
        {
            _httpService = httpService;
            _authenticationService = authenticationService;
        }
        
        [Authorize]
        [HttpGet("/add-product")]
        public IActionResult AddProductGet()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddProduct()
        {
            var claims = _authenticationService.DecryptClaim();
            var token = claims.FirstOrDefault()?.Value;
            
            var response = await _httpService.Post("products", new CreateProjectParameter
            {
                Title = HttpContext.Request.Form["title"],
                Details = HttpContext.Request.Form["details"],
                Price = decimal.Parse(HttpContext.Request.Form["price"]),
                Category = HttpContext.Request.Form["category"]
            }, token);

            if (response.IsSuccessStatusCode) return RedirectToAction("Index", "Home");
           
            ViewData["Error"] = "Error";
           
            return RedirectToAction("Index", "Home");
        }
    }
}