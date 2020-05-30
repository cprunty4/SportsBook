using SportsBook.Interfaces;
using SportsBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsBook.Repository
{
    public class MockTeamRepository : ITeamRepository
    {
        public List<Team> GetAllTeams()
        {
            return new List<Team>
            {
                new Team
                {
                    Id = 1,
                    Location = "Green Bay",
                    TeamAbbreviation = "GBP",
                    Name = "Packers",
                    Conference = "NFC",
                    Division = "NFC North",
                    Colors = new List<string> {"DarkGreen", "Gold"}
                },
                new Team
                {
                    Id = 2,
                    Location = "Kansas City",
                    TeamAbbreviation = "KC",
                    Name = "Chiefs",
                    Conference = "AFC",
                    Division = "AFC West",
                    Colors = new List<string> {"Red", "Gold"}
                },
                new Team
                {
                    Id = 3,
                    Location = "Dallas",
                    TeamAbbreviation = "DAL",
                    Name = "Cowboys",
                    Conference = "NFC",
                    Division = "NFC East",
                    Colors = new List<string> {"NavyBlue", "Silver", "RoyalBlue"}
                }
            };
        }

        public Team GetTeamById(long teamId)
        {
            return this.GetAllTeams().Where(x => x.Id == teamId).FirstOrDefault();
        }
    }
}
