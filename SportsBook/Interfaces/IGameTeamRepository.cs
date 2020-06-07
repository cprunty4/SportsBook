using System.Collections.Generic;
using SportsBook.Models.Database;

namespace SportsBook.Interfaces
{
    public interface IGameTeamRepository
    {
         List<GameTeam> AllGameTeams {get;}

         GameTeam GetById(long gameTeamId);
    }
}