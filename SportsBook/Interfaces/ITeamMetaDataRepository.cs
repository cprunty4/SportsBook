using System.Collections.Generic;
using SportsBook.Models.Views;

namespace SportsBook.Interfaces
{
    public interface ITeamMetaDataRepository
    {
        List<TeamMetaData> AllTeamsMetaData { get;}

    }
}