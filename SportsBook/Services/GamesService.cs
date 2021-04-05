using Microsoft.Extensions.Configuration;
using SportsBook.Interfaces;
using SportsBook.Models;

namespace SportsBook.Services
{
    public class GamesService : IGamesService
    {
        private string baseUrl = string.Empty;
        private readonly IConfiguration _configuration;
        public GamesService(IConfiguration configuration) {
            _configuration = configuration;
            baseUrl = _configuration.GetSection("EntitiesApiOptions").GetValue<string>("BaseUrl");             
        }
        public GamesSearchResponse GetGamesSearch(GamesSearchRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}