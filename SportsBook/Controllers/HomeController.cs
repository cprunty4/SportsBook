using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SportsBook.Interfaces;
using SportsBook.Models;
using SportsBook.Entities;
using SportsBook.Repository;

namespace SportsBook.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITeamRepository _teamRepository;
        private readonly IGameSlateRepository _gameSlateRepository;

        private readonly ITeamMetaDataRepository _teamMetaDataRepository;

        public HomeController(ILogger<HomeController> logger,
            ITeamRepository teamRepository,
            IGameSlateRepository gameSlateRepository,
            ITeamMetaDataRepository teamMetaDataRepository)
        {
            _logger = logger;
            _teamRepository = teamRepository;
            _gameSlateRepository = gameSlateRepository;
            _teamMetaDataRepository = teamMetaDataRepository;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("entered Home controller");
            return View(_teamRepository.AllTeams);
        }

        public IActionResult Privacy()
        {
            ViewBag.Message1 = "This is using Watch";
            return View();
        }

        public IActionResult GameSlate()
        {
            _logger.LogInformation("entered GameSlate action");
            return View(_gameSlateRepository.AllGameSlates);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
