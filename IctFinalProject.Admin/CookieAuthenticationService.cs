using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace IctFinalProject.Admin
{
    public class CookieAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpService _httpService;

        public CookieAuthenticationService(IHttpContextAccessor httpContext, HttpService httpService)
        {
            this._httpContextAccessor = httpContext;
            _httpService = httpService;
        }

        public async Task<bool> AuthenticateUser(string token)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Hash, token)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = false,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1),
                RedirectUri = "/"
            };

            await _httpContextAccessor.HttpContext.SignInAsync(
                Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return true;
        }

        public async void SignOutUser()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(
                Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public string DecryptClaim()
        {
            var opt = _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IOptionsMonitor<Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions>>();
            var cookie = opt.CurrentValue.CookieManager.GetRequestCookie(_httpContextAccessor.HttpContext, ".AspNetCore.Cookies");

            if (string.IsNullOrEmpty(cookie)) return null;
            var dataProtector = opt.CurrentValue.DataProtectionProvider.CreateProtector("Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationMiddleware", Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme, "v2");

            var ticketDataFormat = new TicketDataFormat(dataProtector);
            var ticket = ticketDataFormat.Unprotect(cookie);
            var claims = ticket.Principal.Claims;
            var list = claims.ToList();

            return list[0].Value;
        }
        
    }

}