using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using IctFinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ictFinalProject.WebAdmin.Models;
using Newtonsoft.Json;

namespace ictFinalProject.WebAdmin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpService _httpService;

        public HomeController(ILogger<HomeController> logger, HttpService httpService)
        {
            _logger = logger;
            _httpService = httpService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpService.Get($"api/Subject/");

            var products = response.Content.ReadFromJsonAsync<Product>();

            ViewData["Products"] = products;
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}