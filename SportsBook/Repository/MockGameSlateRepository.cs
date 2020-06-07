using System;
using System.Collections.Generic;
using SportsBook.Interfaces;
using SportsBook.Models.Views;

namespace SportsBook.Repository
{
    public class MockGameSlateRepository : IGameSlateRepository
    {
        private readonly IGameRepository _gameRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IGameTeamRepository _gameTeamRepository;
        private readonly IStadiumRepository _stadiumRepository;

        public MockGameSlateRepository(IGameRepository gameRepository, 
            ITeamRepository teamRepository,
            IGameTeamRepository gameTeamRepository,
            IStadiumRepository stadiumRepository)
        {
            _gameRepository = gameRepository;
            _teamRepository = teamRepository;
            _gameTeamRepository = gameTeamRepository;
            _stadiumRepository = stadiumRepository;
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

                var awayTeam = _teamRepository.GetTeamById(awayGameTeam.TeamId);
                var homeTeam = _teamRepository.GetTeamById(homeGameTeam.TeamId);

                gameSlate.AwayTeamName = awayTeam.TeamName;
                gameSlate.HomeTeamName = homeTeam.TeamName;

                gameSlate.GameStartDateTime = game.StartDateTime;
                gameSlate.AwayTeamSpread = awayGameTeam.SpreadCurrent;
                gameSlate.HomeTeamSpread = homeGameTeam.SpreadCurrent;

                gameSlate.WeekNumber = game.WeekNumber;
                gameSlate.SeasonYear = game.SeasonYear;

                gameSlates.Add(gameSlate);
            }

            return gameSlates;
        }
    }
}