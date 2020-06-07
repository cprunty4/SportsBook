using System.Collections.Generic;
using SportsBook.Models.Views;

namespace SportsBook.Interfaces
{
    public interface IGameSlateRepository
    {
        List<GameSlate> AllGameSlates { get; }
    }
}