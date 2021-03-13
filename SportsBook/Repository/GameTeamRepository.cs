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
    public class GameTeamRepository : IGameTeamRepository
    {
        private readonly IConfiguration _configuration;
        private string baseUrl = string.Empty;

        public GameTeamRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            baseUrl = _configuration.GetSection("EntitiesApiOptions").GetValue<string>("BaseUrl");
        }

        public List<GameTeam> AllGameTeams
        {
            get
            {
                List<GameTeam> gameTeams = new List<GameTeam>();

                var client = new HttpClient();

                string response = client.GetStringAsync($"{baseUrl}/api/GameTeams").Result;

                gameTeams = JsonConvert.DeserializeObject<List<GameTeam>>(response);

                return gameTeams;
            }
        }

        public GameTeam GetById(long gameTeamId)
        {
            GameTeam gameTeam = new GameTeam();

            var client = new HttpClient();

            string response = client.GetStringAsync($"{baseUrl}/api/GameTeams/{gameTeamId}").Result;

            gameTeam = JsonConvert.DeserializeObject<GameTeam>(response);

            return gameTeam;
        }
    }
}
