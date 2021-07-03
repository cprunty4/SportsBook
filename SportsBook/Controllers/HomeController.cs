﻿using System;
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
        private readonly IGamesService _gamesService;

        private readonly ITeamMetaDataService _teamMetaDataService;

        public HomeController(ILogger<HomeController> logger,
            ITeamMetaDataService teamMetaDataService,
            IGamesService gamesService)
        {
            _logger = logger;
            _gamesService = gamesService;
            _teamMetaDataService = teamMetaDataService;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("entered Home controller");
            return View(_teamMetaDataService.GetAllTeams());
        }

        public IActionResult Privacy()
        {
            ViewBag.Message1 = "This is using Watch";
            return View();
        }

        public IActionResult GameSlate()
        {
            _logger.LogInformation("entered GameSlate action");
            GamesSearchRequest request = new GamesSearchRequest{
                pagingOptions = new PagingOptionsModel {
                    Page=1,
                    PageSize=10
                }
            };
            GamesSearchResponse response = _gamesService.GetGamesSearch(request);
            return View(response.GameSlates);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
