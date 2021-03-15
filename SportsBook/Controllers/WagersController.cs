using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SportsBook.Entities;
using SportsBook.Interfaces;
using SportsBook.Models;
using System.Linq;

namespace SportsBook.Controllers
{
    public class WagersController : Controller
    {
        private readonly ILogger<WagersController> _logger;
        private readonly IGameSlateRepository _gameSlateRepository;
        readonly IGameTeamRepository _gameTeamRepository;
        readonly ITeamRepository _teamRepository;

        public WagersController(ILogger<WagersController> logger,
            IGameSlateRepository gameSlateRepository,
            IGameTeamRepository gameTeamRepository,
            ITeamRepository teamRepository
        
        )
        {
            _logger = logger;
            _gameSlateRepository = gameSlateRepository;
            _gameTeamRepository = gameTeamRepository;
            _teamRepository = teamRepository;
        }

        // GET: create/5
        [HttpGet("wagers/create/{gameTeamId}")]
        public IActionResult Create([FromRoute] long gameTeamId, [FromQuery] long gameId) {

            GameTeam gameTeam = _gameTeamRepository.GetById(gameTeamId);
            Team team = _teamRepository.GetTeamById(gameTeam.TeamId);

            // TODO create call GetGameSlateById()
            GameSlate gameSlate = _gameSlateRepository.AllGameSlates.Where(x => x.GameId == gameId).SingleOrDefault();

            CreateWager createWager = new CreateWager {
                AwayTeamFullName = gameSlate.AwayTeamFullName,
                HomeTeamFullName = gameSlate.HomeTeamFullName,
                WagerAmount = 11.00m,
                WagerAmountToWin = 10.00m,
                WagerGameTeamSpreadMoneylineOfBet = gameTeam.SpreadMoneylineCurrent,
                WagerGameTeamTeamName = team.FullName,
                WagerGameTeamSpreadOfBet = gameTeam.SpreadCurrent
            };

            return View(createWager);
        }
        
    }
}