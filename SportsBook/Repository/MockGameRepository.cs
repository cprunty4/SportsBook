using System;
using System.Collections.Generic;
using System.Linq;
using SportsBook.Interfaces;
using SportsBook.Models.Database;

namespace SportsBook.Repository
{
    public class MockGameRepository : IGameRepository
    {
        public List<Game> GetAllGames()
        {
            return new List<Game> {
                new Game {
                    Id = 1,
                    LeagueId = 1,
                    GameType = GameTypeEnum.RegularSeason,
                    AwayGameTeamId = 1,
                    HomeGameTeamId = 4,
                    StadiumId = 1,
                    SeasonYear = 2020,
                    WeekNumber = 1,
                    StartDateTime = DateTime.Parse("2020-09-13 12:00"),
                    IsFinal = false,
                    IsStarted = false,
                    Weather = string.Empty,
                    Wind = string.Empty,
                    OverUnderCurrent = 46.5m,
                    OverUnderMoneylineCurrent = -110
                    
                }
            };
        }

        public Game GetGameById(long gameId)
        {
            return this.GetAllGames().Where(x => x.Id == gameId).First();
        }
    }
}