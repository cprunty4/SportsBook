using System.Collections.Generic;
using SportsBook.Entities;

namespace SportsBook.Interfaces
{
    public interface ITeamMetaDataService
    {
        List<Team> GetAllTeams();
    }
}