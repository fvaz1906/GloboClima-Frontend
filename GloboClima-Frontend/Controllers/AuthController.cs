using GloboClima_Frontend.Models;
using GloboClima_Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace GloboClima_Frontend.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly ApiService _apiService;

        public AuthController(
            ILogger<AuthController> logger,
            ApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

        public async Task<IActionResult> Login()
        {
            return await Task.FromResult(View());
        }

        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("AuthToken");

            return await Task.FromResult(RedirectToAction("Index", "Home"));
        }

        public async Task<IActionResult> LoginUser(User user)
        {
            if (await _apiService.Login(user))
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Register()
        {
            return await Task.FromResult(View());
        }

        public async Task<IActionResult> RegisterUser(User user)
        {
            if (await _apiService.Register(user))
            {
                return RedirectToAction("Login", "Auth");
            }

            return RedirectToAction("Login");
        }
    }
}
