using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SportsBook.Entities;
using SportsBook.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SportsBook.Repository
{
    public class GameRepository : IGameRepository
    {
        private readonly IConfiguration _configuration;
        private string baseUrl = string.Empty;

        public GameRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            baseUrl = _configuration.GetSection("EntitiesApiOptions").GetValue<string>("BaseUrl");
        }

        public List<Game> AllGames
        {
            get {
                List<Game> games = new List<Game>();

                var client = new HttpClient();

                string response = client.GetStringAsync($"{baseUrl}/api/Games").Result;

                games = JsonConvert.DeserializeObject<List<Game>>(response);

                return games;
            }
        }

        public Game GetGameById(long gameId)
        {
            Game game = new Game();

            var client = new HttpClient();

            string response = client.GetStringAsync($"{baseUrl}/api/Games/{gameId}").Result;

            game = JsonConvert.DeserializeObject<Game>(response);

            return game;
        }
    }
}
