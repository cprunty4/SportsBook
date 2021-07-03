using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SportsBook.Interfaces;
using SportsBook.Models;

namespace SportsBook.Controllers
{
    public class GamesController : Controller
    {
        private readonly ILogger<GamesController> _logger;
        private IGamesService _gamesService;

        public GamesController(ILogger<GamesController> logger,
            IGamesService gamesService
        )
        {
            _logger = logger;
            _gamesService = gamesService;
        }
        public IActionResult GamesSearch()
        {
            GamesSearchRequest request = new GamesSearchRequest();
            GamesSearchResponse response = _gamesService.GetGamesSearch(request);
            _logger.LogInformation("entered gamesController");
            return View(response.GameSlates);
        }        
    }
}