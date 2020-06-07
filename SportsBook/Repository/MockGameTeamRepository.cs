using System.Collections.Generic;
using System.Linq;
using SportsBook.Interfaces;
using SportsBook.Models.Database;

namespace SportsBook.Repository
{
    public class MockGameTeamRepository : IGameTeamRepository
    {
        public List<GameTeam> AllGameTeams =>
            new List<GameTeam> {
                new GameTeam {
                    Id = 1,
                    TeamId = 1,
                    Score = null,
                    SpreadCurrent = 3.5m,
                    SpreadMoneylineCurrent = -110,
                    MoneyLineCurrent = 158
                },
                new GameTeam {
                    Id = 2,
                    TeamId = 4,
                    SpreadCurrent = -3.5m,
                    SpreadMoneylineCurrent = -110,
                    MoneyLineCurrent = 186
                }
            };

        public GameTeam GetById(long gameTeamId)
        {
            return this.AllGameTeams.Where(x => x.Id == gameTeamId).First();
        }
    }
}