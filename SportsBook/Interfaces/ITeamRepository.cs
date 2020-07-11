using SportsBook.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsBook.Interfaces
{
    public interface ITeamRepository
    {
        List<Team> AllTeams { get; }
        Team GetTeamById(long teamId);

    }
}
