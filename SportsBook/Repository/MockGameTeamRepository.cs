using System.Collections.Generic;
using System.Linq;
using SportsBook.Interfaces;
using SportsBook.Entities;

namespace SportsBook.Repository
{
    public class MockGameTeamRepository : IGameTeamRepository
    {
        public List<GameTeam> AllGameTeams =>
            new List<GameTeam> {
                new GameTeam {
                    Id = 1,
                    TeamId = 1,
                    Score = 43,
                    SpreadCurrent = 3.5m,
                    SpreadMoneylineCurrent = -110,
                    MoneyLineCurrent = 158
                },
                new GameTeam {
                    Id = 2,
                    TeamId = 4,
                    Score = 34,
                    SpreadCurrent = -3.5m,
                    SpreadMoneylineCurrent = -110,
                    MoneyLineCurrent = 186
                },
                new GameTeam {
                    Id = 3,
                    TeamId = 1,
                    Score = 42,
                    SpreadCurrent = -5.5m,
                    SpreadMoneylineCurrent = -110,
                    MoneyLineCurrent = -270
                },
                new GameTeam {
                    Id = 4,
                    TeamId = 14,
                    SpreadCurrent = 5.5m,
                    SpreadMoneylineCurrent = -110,
                    MoneyLineCurrent = 220,
                    Score =21
                },
                new GameTeam {
                    Id = 5,
                    TeamId = 1,
                    SpreadCurrent = 4.5m,
                    SpreadMoneylineCurrent = -115,
                    MoneyLineCurrent = 158,
                    Score =37
                },
                new GameTeam {
                    Id = 6,
                    TeamId = 13,
                    SpreadCurrent = -4.5m,
                    SpreadMoneylineCurrent = -105,
                    MoneyLineCurrent = -188,
                    Score =30
                },
                new GameTeam {
                    Id = 7,
                    TeamId = 1,
                    SpreadCurrent = -5.0m,
                    SpreadMoneylineCurrent = -105,
                    MoneyLineCurrent = -188,
                    Score =30
                },    
                new GameTeam {
                    Id = 8,
                    TeamId = 15,
                    SpreadCurrent = 5.0m,
                    SpreadMoneylineCurrent = -105,
                    MoneyLineCurrent = 158,
                    Score =16
                },
                new GameTeam {
                    Id = 9,
                    TeamId = 1,
                    SpreadCurrent = -1.0m,
                    SpreadMoneylineCurrent = -110,
                    MoneyLineCurrent = -112,
                    Score =null
                },
                new GameTeam {
                    Id = 10,
                    TeamId = 16,
                    SpreadCurrent = 1.0m,
                    SpreadMoneylineCurrent = -110,
                    MoneyLineCurrent = -104,
                    Score =null
                }                                 

            };

        public GameTeam GetById(long gameTeamId)
        {
            return this.AllGameTeams.Where(x => x.Id == gameTeamId).First();
        }
    }
}