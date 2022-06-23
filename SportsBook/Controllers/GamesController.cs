using System;
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
            _logger.LogInformation("entered gamesController");

            DateTime startDate = DateTime.Parse("2022-02-28");
            DateTime endDate = DateTime.Parse("2023-02-28");

            GamesSearchRequest request = new GamesSearchRequest
            {
                startDate=startDate,
                endDate=endDate,
                seasonYear=2022,
                pagingOptions = new PagingOptionsModel
                {
                    Page = 1,
                    PageSize = 10
                }
            };
            GamesSearchResponse response = _gamesService.GetGamesSearchApi(request);
            return View("GameSlate", response.GameSlates);
        }

        [HttpPost]
        public IActionResult Search()
        {
            string strStartDate = Request.Form["startDate"];
            string strEndDate = Request.Form["endDate"];
            string teamName = Request.Form["teamName"];

            DateTime startDate;
            DateTime endDate;

            DateTime.TryParse(strStartDate, out startDate);
            DateTime.TryParse(strEndDate, out endDate);

            GamesSearchRequest request = new GamesSearchRequest
            {
                startDate = startDate,
                endDate = endDate,
                teamName = teamName,
                pagingOptions = new PagingOptionsModel
                {

                }
            };


            GamesSearchResponse response = _gamesService.GetGamesSearch(request);

            return View(response.GameSlates);
        }
    }
}