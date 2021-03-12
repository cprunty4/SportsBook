using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SportsBook.Entities;
using SportsBook.Interfaces;
using SportsBook.Repository.Mappers;

namespace SportsBook.Repository
{
    public class TeamRepository : ITeamRepository
    {

        private readonly IConfiguration _configuration;
        private int _entityTypeId = 1;
        private string baseUrl = string.Empty;           

        public TeamRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            baseUrl = _configuration.GetSection("EntitiesApiOptions").GetValue<string>("BaseUrl");            
            _entityTypeId = _configuration.GetSection("EntitiesApiOptions").GetValue<int>("EntityTypeId");
        }

        public List<Team> AllTeams
        {
            get
            {
                List<Entity> entities = new List<Entity>();
                List<Team> teams = new List<Team>();

                var client = new HttpClient();

                string response = client.GetStringAsync($"{baseUrl}/api/Entities?entityTypeId={_entityTypeId}").Result;

                entities = JsonConvert.DeserializeObject<List<Entity>>(response);

                teams = EntityMapper.MapEntityToTeams(entities);

                return teams;
            }
        }

        public Team GetTeamById(long teamId)
        {
            Team team = new Team();
            var client = new HttpClient();
            Entity entity = new Entity();

            string response = client.GetStringAsync($"{baseUrl}/api/Entities/{teamId}").Result;

            entity = JsonConvert.DeserializeObject<Entity>(response);

            team = EntityMapper.MapEntityToTeam(entity);

            return team;
        }
    }
}