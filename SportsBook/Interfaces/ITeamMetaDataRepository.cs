using System.Collections.Generic;
using SportsBook.Models;

namespace SportsBook.Interfaces
{
    public interface ITeamMetaDataRepository
    {
        List<TeamMetaData> AllTeamsMetaData { get;}

    }
}