using SportsBook.Interfaces;
using SportsBook.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsBook.Repository
{
    public class MockTeamRepository : ITeamRepository
    {
        public List<Team> AllTeams => 
        
            new List<Team>
            {
                new Team
                {
                    Id = 1,
                    Location = "Green Bay",
                    TeamAbbreviation = "GBP",
                    Name = "Packers",
                    Conference = "NFC",
                    Division = "NFC North",
                    Colors = new List<string> {"DarkGreen", "Gold"},
                    HomeStadiumId = 1,
                    HelmetImageFileName = "green-bay-packers-helmet3.jpg"
                },
                new Team
                {
                    Id = 2,
                    Location = "Kansas City",
                    TeamAbbreviation = "KC",
                    Name = "Chiefs",
                    Conference = "AFC",
                    Division = "AFC West",
                    Colors = new List<string> {"Red", "Gold"},
                    HomeStadiumId = 2,
                    HelmetImageFileName = "kansas-city-chiefs-helmet.jpg"
                },
                new Team
                {
                    Id = 3,
                    Location = "Dallas",
                    TeamAbbreviation = "DAL",
                    Name = "Cowboys",
                    Conference = "NFC",
                    Division = "NFC East",
                    Colors = new List<string> {"NavyBlue", "Silver", "RoyalBlue"},
                    HelmetImageFileName = "dallas-cowboys-helmet.jpg"
                },
                new Team
                {
                    Id = 4,
                    Location = "Minnesota",
                    TeamAbbreviation = "MIN",
                    Name = "Vikings",
                    Conference = "NFC",
                    Division = "NFC North",
                    Colors = new List<string> {"Purple", "Gold"},
                    HomeStadiumId = 4,
                    HelmetImageFileName = "minnesota-vikings-helmet2.jpg"
                }                
            };


        public Team GetTeamById(long teamId)
        {
            return this.AllTeams.Where(x => x.Id == teamId).FirstOrDefault();
        }
    }
}
