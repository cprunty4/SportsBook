using System;
using System.Collections.Generic;
using System.Linq;
using SportsBook.Interfaces;
using SportsBook.Entities;

namespace SportsBook.Repository
{
    public class MockGameRepository : IGameRepository
    {
        public List<Game> AllGames =>
        new List<Game> {
            new Game {
                Id = 1,
                LeagueId = 1,
                GameType = GameTypeEnum.RegularSeason,
                AwayGameTeamId = 1,
                HomeGameTeamId = 2,
                StadiumId = 4,
                SeasonYear = 2020,
                WeekNumber = 1,
                StartDateTime = DateTime.Parse("2020-09-13 12:00"),
                IsFinal = true,
                IsStarted = false,
                Weather = string.Empty,
                Wind = string.Empty,
                OverUnderCurrent = 46.5m,
                OverUnderMoneylineCurrent = -110

            },
            new Game {
                Id = 2,
                LeagueId = 1,
                GameType = GameTypeEnum.RegularSeason,
                AwayGameTeamId = 4,
                HomeGameTeamId = 3,
                StadiumId = 1,
                SeasonYear = 2020,
                WeekNumber = 2,
                StartDateTime = DateTime.Parse("2020-09-20 12:00"),
                IsFinal = true,
                IsStarted = false,
                Weather = string.Empty,
                Wind = string.Empty,
                OverUnderCurrent = 47.5m,
                OverUnderMoneylineCurrent = -110

            },
            new Game {
                Id = 3,
                LeagueId = 1,
                GameType = GameTypeEnum.RegularSeason,
                AwayGameTeamId = 6,
                HomeGameTeamId = 5,
                StadiumId = 5,
                SeasonYear = 2020,
                WeekNumber = 3,
                StartDateTime = DateTime.Parse("2020-09-27 19:20"),
                IsFinal = false,
                IsStarted = false,
                Weather = string.Empty,
                Wind = string.Empty,
                OverUnderCurrent = 0,
                OverUnderMoneylineCurrent = -110

            }              
        };

        public Game GetGameById(long gameId)
        {
            return this.AllGames.Where(x => x.Id == gameId).First();
        }
    }
}