using System.Collections.Generic;
using SportsBook.Models;

namespace SportsBook.Interfaces
{
    public interface IGameSlateRepository
    {
        List<GameSlate> AllGameSlates { get; }

        GameSlate GetByGameId(long gameId);
    }
}