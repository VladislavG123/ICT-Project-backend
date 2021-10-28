using System;
using System.Threading.Tasks;
using IctFinalProject.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ictFinalProject.WebAdmin.Controllers
{
    public class AuthController : Controller
    {
        private readonly CookieAuthenticationService _authenticationService;
        private readonly HttpService _httpService;

        public AuthController(CookieAuthenticationService authenticationService, HttpService httpService)
        {
            _authenticationService = authenticationService;
            _httpService = httpService;
        }
        
        // GET
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            string password = HttpContext.Request.Form["password"];
            
            var response = await _httpService.Post("admin/identity", new IdentityParameter
            {
                Password = password
            });

            if (!response.IsSuccessStatusCode)
            {
                throw new ArgumentException("PhoneNumber or password is invalid");
            }

            var token = await response.Content.ReadAsStringAsync();

            var isAuthenticated = await _authenticationService.AuthenticateUser(token);
            
            return RedirectToRoute("/Index");
        }
    }
}