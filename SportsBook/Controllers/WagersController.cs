using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SportsBook.Entities;
using SportsBook.Interfaces;
using SportsBook.Models;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace SportsBook.Controllers
{
    public class WagersController : Controller
    {
        private readonly ILogger<WagersController> _logger;
        private readonly IGameSlateRepository _gameSlateRepository;
        readonly IGameTeamRepository _gameTeamRepository;
        readonly ITeamRepository _teamRepository;
        private string baseUrl = string.Empty;
        private readonly IConfiguration _configuration;

        public WagersController(ILogger<WagersController> logger,
            IGameSlateRepository gameSlateRepository,
            IGameTeamRepository gameTeamRepository,
            ITeamRepository teamRepository,
            IConfiguration configuration
        )
        {
            _logger = logger;
            _gameSlateRepository = gameSlateRepository;
            _gameTeamRepository = gameTeamRepository;
            _teamRepository = teamRepository;
            _configuration = configuration;
            baseUrl = _configuration.GetSection("EntitiesApiOptions").GetValue<string>("BaseUrl");
        }

        // GET: create/5
        [HttpGet("wagers/create/{gameTeamId}")]
        public IActionResult Create([FromRoute] long gameTeamId, [FromQuery] long gameId, [FromQuery] int? wagerType) {

            GameTeam gameTeam = _gameTeamRepository.GetById(gameTeamId);
            Team team = _teamRepository.GetTeamById(gameTeam.TeamId);
            GameSlate gameSlate = _gameSlateRepository.GetByGameId(gameId);

            CreateWager createWager = new CreateWager {
                AwayTeamFullName = gameSlate.AwayTeamFullName,
                HomeTeamFullName = gameSlate.HomeTeamFullName,
                WagerGameTeamSpreadMoneylineOfBet = $"{(gameTeam.SpreadMoneylineCurrent > 0 ? "+" : string.Empty)}{gameTeam.SpreadMoneylineCurrent}",
                WagerGameTeamTeamName = team.FullName,
                WagerGameTeamSpreadOfBet = $"{(gameTeam.SpreadCurrent > 0 ? "+" : string.Empty)}{gameTeam.SpreadCurrent}",
                WagerType = wagerType,
                GameTeamId = gameTeamId,
                GameId = gameId
            };

            return View(createWager);
        }

        [HttpPost]
        public IActionResult Create([Bind("GameTeamId,WagerGameTeamSpreadMoneylineOfBet,WagerGameTeamSpreadOfBet,WagerType,WagerAmount,WinAmount,PayoutAmount,UpdatedBy,WagerGameTeamTeamName,GameId")] CreateWager createWager)
        {
            _logger.LogInformation("entered create wager");

            var client = new HttpClient();
            Wager wager = new Wager
            {
                GameId = createWager.GameId,
                GameTeamId = createWager.GameTeamId,
                WagerType = createWager.WagerType,
                WagerAmount = createWager.WagerAmount,
                WinAmount = createWager.WinAmount,
                PayoutAmount = createWager.PayoutAmount,
                TeamName = createWager.WagerGameTeamTeamName,
                SpreadOfBet = createWager.WagerGameTeamSpreadOfBet,
                SpreadMoneylineOfBet = createWager.WagerGameTeamSpreadMoneylineOfBet

            };
            string json = JsonConvert.SerializeObject(wager);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync($"{baseUrl}/api/Wagers", stringContent).Result;

            return View("Confirm", createWager);
        }

    }
}