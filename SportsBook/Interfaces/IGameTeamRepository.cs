using System.Collections.Generic;
using SportsBook.Entities;

namespace SportsBook.Interfaces
{
    public interface IGameTeamRepository
    {
         List<GameTeam> AllGameTeams {get;}

         GameTeam GetById(long gameTeamId);
    }
}