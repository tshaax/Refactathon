using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GoLMvcFrontEnd.Models;
using GameOfLife;
using Microsoft.Extensions.Configuration;

namespace GoLMvcFrontEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> GetGameOfLife(int width, int height, int generations)
        {
            using var handler = new HttpClientHandler{ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true};
            using var client = new HttpClient(handler){BaseAddress = new Uri(_configuration.GetValue<string>("LocalBaseUrl")) };
            var request =
                await client.GetAsync(
                    $"api/GameOfLifeData/GetAsStrings?width={width}&height={height}&generations={generations}");
            var result = JsonSerializer.Deserialize<List<string>>(await request.Content.ReadAsStringAsync());
            return Ok(result);
        }
    }
}
