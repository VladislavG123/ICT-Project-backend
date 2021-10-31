using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using IctFinalProject.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ictFinalProject.WebAdmin.Models;
using Microsoft.AspNetCore.Authorization;

namespace ictFinalProject.WebAdmin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpService _httpService;
        private readonly CookieAuthenticationService _authenticationService;

        public HomeController(ILogger<HomeController> logger, HttpService httpService, 
            CookieAuthenticationService authenticationService)
        {
            _logger = logger;
            _httpService = httpService;
            _authenticationService = authenticationService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var response = await _httpService.Get($"products?ShowInactive=true");

            var products = await response.Content.ReadFromJsonAsync<List<Product>>();

            ViewData["Products"] = products;
            
            return View();
        }

        [Authorize]
        [HttpGet("/change-activity/{id:guid}")]
        public async Task<IActionResult> ChangeActivity(Guid id)
        {
            var claims = _authenticationService.DecryptClaim();
            var token = claims.FirstOrDefault()?.Value;
            
            var response = await _httpService.Patch($"products/{id}/change_isActive", null, token);

            if (response.IsSuccessStatusCode) return RedirectToAction("Index", "Home");
           
            ViewData["Error"] = "Error";
           
            return RedirectToAction("Index", "Home");
        }
        
        [Authorize]
        [HttpGet("/delete/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var claims = _authenticationService.DecryptClaim();
            var token = claims.FirstOrDefault()?.Value;
            
            var response = await _httpService.Delete($"products/{id}", token);

            if (response.IsSuccessStatusCode) return RedirectToAction("Index", "Home");
           
            ViewData["Error"] = "Error";
           
            return RedirectToAction("Index", "Home");
        }
    }
}