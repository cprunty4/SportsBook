using SportsBook.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsBook.Interfaces
{
    public interface IGameRepository
    {
        List<Game> AllGames { get; }
        Game GetGameById(long gameId);
    }
}
