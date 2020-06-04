using SportsBook.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsBook.Interfaces
{
    public interface IGameRepository
    {
        List<Game> GetAllGames();
        Game GetGameById(long gameId);
    }
}
