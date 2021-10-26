using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IctFinalProject.Admin.Pages
{
    public class AuthPage : PageModel
    {
        private readonly CookieAuthenticationService _adminAuthentication;
        private readonly HttpService _httpService;

        public AuthPage(CookieAuthenticationService adminAuthentication, HttpService httpService)
        {
            _adminAuthentication = adminAuthentication;
            _httpService = httpService;
        }

        
        public void OnGet()
        {
            
        }
        
        public async Task<IActionResult> OnPostAsync([FromForm] string password)
        {
            try
            {
                var response = await _httpService.Post("admin/identity", new
                {
                    Password = password
                });

                if (!response.IsSuccessStatusCode)
                {
                    throw new ArgumentException("PhoneNumber or password is invalid");
                }

                var token = await response.Content.ReadAsStringAsync();

                await _adminAuthentication.AuthenticateUser(token);
            }
            catch (ArgumentException e)
            {
                ViewData["ShowAlert"] = true;
                return Page();
            }
            
            return Redirect("/");
        } 

    }
}