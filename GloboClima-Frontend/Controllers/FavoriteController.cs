using GloboClima_Frontend.Models;
using GloboClima_Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace GloboClima_Frontend.Controllers
{
    public class FavoriteController : Controller
    {

        private readonly ILogger<FavoriteController> _logger;
        private readonly ApiService _apiService;

        public FavoriteController(
            ILogger<FavoriteController> logger,
            ApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            List<Favorite>? favorites = await _apiService.GetFavoritesAsync();

            return View(favorites);
        }

        public async Task<IActionResult> Save(Favorite favorite)
        {
            if (favorite == null)
            {
                return BadRequest("O identificador do favorito é inválido.");
            }

            var result = await _apiService.SaveFavoritesAsync(favorite);

            if (result != null)
            {
                TempData["SuccessMessage"] = "Favorito salvo com sucesso.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Não foi possível salvar o favorito.";
                return RedirectToAction("Index");
            }

        }

        public async Task<IActionResult> Delete(string hashId)
        {
            if (string.IsNullOrEmpty(hashId))
            {
                return BadRequest("O identificador do favorito é inválido.");
            }

            var result = await _apiService.DeleteFavoritesAsync(hashId);

            if (result != null)
            {
                TempData["SuccessMessage"] = "Favorito removido com sucesso.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Não foi possível deletar o favorito.";
                return RedirectToAction("Index");
            }

        }

    }
}
