using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ictFinalProject.WebAdmin
{
    public class CookieAuthenticationService
    {
       private readonly IHttpContextAccessor httpContextAccessor;

        public CookieAuthenticationService(IHttpContextAccessor httpContext)
        {
            this.httpContextAccessor = httpContext;
        }

        public async Task<bool> AuthenticateUser(string token)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Hash, token)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = false,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                RedirectUri = "/Home/Index"
            };

            await httpContextAccessor.HttpContext.SignInAsync(
                Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);


            return true;
        }

        public async void SignOutUser()
        {
            await httpContextAccessor.HttpContext.SignOutAsync(
                Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public List<Claim> DecryptClaim()
        {
            // Get the encrypted cookie value
            var opt = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IOptionsMonitor<Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions>>();
            var cookie = opt.CurrentValue.CookieManager.GetRequestCookie(httpContextAccessor.HttpContext, ".AspNetCore.Cookies");

            // Decrypt if found
            if (string.IsNullOrEmpty(cookie)) return null;
            
            var dataProtector = opt.CurrentValue.DataProtectionProvider.CreateProtector("Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationMiddleware", Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme, "v2");

            var ticketDataFormat = new TicketDataFormat(dataProtector);
            var ticket = ticketDataFormat.Unprotect(cookie);
            var claims = ticket.Principal.Claims;
            var list = claims.ToList();

            return list;
        }
        
    }

}