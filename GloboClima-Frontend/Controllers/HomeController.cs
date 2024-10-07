using GloboClima_Frontend.Models;
using GloboClima_Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GloboClima_Frontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApiService _apiService;

        public HomeController(
            ILogger<HomeController> logger,
            ApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> SearchWeather(string search)
        {
            Weather? weather = await _apiService.GetWeatherAsync(search);

            return View(weather);
        }

        public async Task<IActionResult> SearchCountry(string search)
        {
            Country? countryData = await _apiService.GetCountryAsync(search);

            return View(countryData);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
