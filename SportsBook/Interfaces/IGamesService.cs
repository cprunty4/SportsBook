using SportsBook.Models;

namespace SportsBook.Interfaces
{
    public interface IGamesService
    {
         GamesSearchResponse GetGamesSearch(GamesSearchRequest request);
        GamesSearchResponse GetGamesSearchApi(GamesSearchRequest request);

    }
}