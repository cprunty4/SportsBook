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
                    LocationName = "Green Bay",
                    TeamAbbreviation = "GBP",
                    MascotName = "Packers",
                    Conference = "NFC",
                    Division = "NFC North"
                }
            };
        }

        public Team GetTeamById(long teamId)
        {
            return this.GetAllTeams().Where(x => x.Id == teamId).FirstOrDefault();
        }
    }
}
