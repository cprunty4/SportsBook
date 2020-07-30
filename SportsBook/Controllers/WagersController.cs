using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SportsBook.Interfaces;
using SportsBook.Models;

namespace SportsBook.Controllers
{
    public class WagersController : Controller
    {
        private readonly ILogger<WagersController> _logger;
        private readonly IGameSlateRepository _gameSlateRepository;        

        public WagersController(ILogger<WagersController> logger,
            IGameSlateRepository gameSlateRepository
        
        )
        {
            _logger = logger;
            _gameSlateRepository = gameSlateRepository;
        }

        [HttpGet]
        public IActionResult Create(long gameId) {
            
            CreateWager createWager = new CreateWager {
                AwayTeamName = "Green Bay Packers",
                HomeTeamName = "Minnesota Vikings"
            };

            return View(createWager);
        }
        
    }
}