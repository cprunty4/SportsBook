using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SportsBook.Entities;
using SportsBook.Interfaces;
using SportsBook.Repository.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SportsBook.Repository
{
    public class StadiumsRepository : IStadiumRepository
    {
        private readonly IConfiguration _configuration;
        private int _entityTypeId = 2;
        private string baseUrl = string.Empty;

        public StadiumsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            baseUrl = _configuration.GetSection("EntitiesApiOptions").GetValue<string>("BaseUrl");
            _entityTypeId = _configuration.GetSection("EntitiesApiOptions").GetValue<int>("EntityTypeId");
        }
        public List<Stadium> AllStadiums 
        {
            get {
                List<Entity> entities = new List<Entity>();
                List<Stadium> stadiums = new List<Stadium>();

                var client = new HttpClient();

                string response = client.GetStringAsync($"{baseUrl}/api/Entities?entityTypeId={_entityTypeId}").Result;

                entities = JsonConvert.DeserializeObject<List<Entity>>(response);

                stadiums = EntityMapper.MapEntityToStadiums(entities);

                return stadiums;
            }
        }

        public Stadium GetStadiumById(long stadiumId)
        {
            Stadium stadium = new Stadium();
            var client = new HttpClient();
            Entity entity = new Entity();

            string response = client.GetStringAsync($"{baseUrl}/api/Entities/{stadiumId}").Result;

            entity = JsonConvert.DeserializeObject<Entity>(response);

            stadium = EntityMapper.MapEntityToStadium(entity);

            return stadium;
        }
    }
}
