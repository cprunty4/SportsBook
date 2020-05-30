using SportsBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsBook.Interfaces
{
    public interface ITeamRepository
    {
        List<Team> GetAllTeams();
        Team GetTeamById(long teamId);

    }
}
