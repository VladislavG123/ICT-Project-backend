using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using IctFinalProject.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ictFinalProject.WebAdmin.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly HttpService _httpService;
        private readonly CookieAuthenticationService _authenticationService;

        public OrderController(HttpService httpService, 
            CookieAuthenticationService authenticationService)
        {
            _httpService = httpService;
            _authenticationService = authenticationService;
        }
        
        // GET
        public async Task<IActionResult> Index()
        {
            var claims = _authenticationService.DecryptClaim();
            var token = claims.FirstOrDefault()?.Value;
            
            var response = await _httpService.Get($"orders", token);

            var orders = await response.Content.ReadFromJsonAsync<List<OrderInfoViewModel>>();

            ViewData["Orders"] = orders;

            return View();
        }

        [HttpGet("/order/{orderId:guid}/set-status/{activity:int}")]
        public async Task<IActionResult> SetActivity(Guid orderId, int activity)
        {
            var uri = $"orders/{orderId}?orderStatus={activity}";

            var claims = _authenticationService.DecryptClaim();
            var token = claims.FirstOrDefault()?.Value;
            
            var response = await _httpService.Patch(uri, null, token);

            return RedirectToAction("Index");
        }
    }
}