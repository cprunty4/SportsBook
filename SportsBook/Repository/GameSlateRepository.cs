using System;
using System.Collections.Generic;
using SportsBook.Interfaces;
using SportsBook.Models;
using System.Linq;

namespace SportsBook.Repository
{
    public class GameSlateRepository : IGameSlateRepository
    {
        private readonly IGameRepository _gameRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IGameTeamRepository _gameTeamRepository;
        private readonly IStadiumRepository _stadiumRepository;
        private readonly IAzureBlobService _azureBlobService;        
        public GameSlateRepository(IGameRepository gameRepository, 
            ITeamRepository teamRepository,
            IGameTeamRepository gameTeamRepository,
            IStadiumRepository stadiumRepository,
            IAzureBlobService azureBlobService)
        {
            _gameRepository = gameRepository;
            _teamRepository = teamRepository;
            _gameTeamRepository = gameTeamRepository;
            _stadiumRepository = stadiumRepository;
            _azureBlobService = azureBlobService;    
        }

        public List<GameSlate> AllGameSlates => this.FetchAllGameSlates();

        private List<GameSlate> FetchAllGameSlates()
        {
            var gameSlates = new List<GameSlate>();

            var teams = _teamRepository.AllTeams;
            var games = _gameRepository.AllGames;
            var gameTeams = _gameTeamRepository.AllGameTeams;
            var stadiums = _stadiumRepository.AllStadiums;

            foreach(var game in games) {
                var gameSlate = new GameSlate();
                
                var awayGameTeam = _gameTeamRepository.GetById(game.AwayGameTeamId);
                var homeGameTeam = _gameTeamRepository.GetById(game.HomeGameTeamId);

                gameSlate.AwayGameTeamId = game.AwayGameTeamId;
                gameSlate.HomeGameTeamId = game.HomeGameTeamId;
                gameSlate.GameId = game.Id;

                var awayTeam = _teamRepository.GetTeamById(awayGameTeam.TeamId);
                var homeTeam = _teamRepository.GetTeamById(homeGameTeam.TeamId);

                gameSlate.AwayTeamFullName = awayTeam.FullName;
                gameSlate.HomeTeamFullName = homeTeam.FullName;

                gameSlate.GameStartDateTime = game.StartDateTime;
                gameSlate.AwayTeamSpread = awayGameTeam.SpreadCurrent;
                gameSlate.HomeTeamSpread = homeGameTeam.SpreadCurrent;

                gameSlate.WeekNumber = game.WeekNumber;
                gameSlate.SeasonYear = game.SeasonYear;
                gameSlate.OverUnder = game.OverUnderCurrent;
                gameSlate.AwayTeamMoneyline = awayGameTeam.MoneyLineCurrent;
                gameSlate.HomeTeamMoneyline = homeGameTeam.MoneyLineCurrent;

                var stadium = _stadiumRepository.GetStadiumById(game.StadiumId);
                gameSlate.StadiumName = $"{stadium.Name}";
                gameSlate.StadiumImageFileName = stadium.StadiumImageFileName;
                gameSlate.LeagueName = "NFL";

                gameSlate.IsFinal = game.IsFinal;
                gameSlate.AwayTeamScore = awayGameTeam.Score;
                gameSlate.HomeTeamScore = homeGameTeam.Score;

                gameSlate.AwayTeamNickname = awayTeam.FullName.Split(' ').LastOrDefault();
                gameSlate.HomeTeamNickname = homeTeam.FullName.Split(' ').LastOrDefault();

                gameSlate.StadiumImageBlobUri = _azureBlobService.GetImageUri(stadium.StadiumImageFileName);

                gameSlates.Add(gameSlate);
            }

            return gameSlates;
        }
    }
}