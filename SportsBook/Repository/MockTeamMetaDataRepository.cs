using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SportsBook.Interfaces;
using SportsBook.Models;

namespace SportsBook.Repository
{
    public class MockTeamMetaDataRepository : ITeamMetaDataRepository
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IConfiguration _configuration;

        private string baseUrl = string.Empty;        

        public MockTeamMetaDataRepository(ITeamRepository teamRepository, IConfiguration configuration)
        {
            _teamRepository = teamRepository;
            _configuration = configuration;

            baseUrl = _configuration.GetSection("EntitiesApiOptions").GetValue<string>("BaseUrl");
        }        
        public List<TeamMetaData> AllTeamsMetaData => this.FetchAllTeamsMetaData();

        private List<TeamMetaData> FetchAllTeamsMetaData()
        {
            var teamsMetaData = new List<TeamMetaData>();

            var teams = _teamRepository.AllTeams;

            foreach(var team in teams)            
            {
                var teamMetaDate = new TeamMetaData();

                teamMetaDate.TeamId = team.Id;
                teamMetaDate.TeamLocation = team.Location;
                teamMetaDate.TeamName = team.Name;
                teamMetaDate.TeamHelmetImageFileName = team.HelmetImageFileName;
                teamMetaDate.NumberOfComments = this.GetNumberOfComments(team.Id);
                teamMetaDate.NumberOfLikes = this.GetNumberOfLikes(team.EntityId);
                teamsMetaData.Add(teamMetaDate);
            }


            return teamsMetaData;
        }

        private int GetNumberOfComments(long id)
        {

            // TODO Call Service here that retrieves data from Entities API
            return 0;
        }

        private int GetNumberOfLikes(int? teamEntityId) {
            // Call Service here that retrieves data from Entities API  
            List<EntityStatusHistory> statusHistory = new List<EntityStatusHistory>();
            if (teamEntityId != null) {
                var client = new HttpClient();            

                string response = client.GetStringAsync($"{baseUrl}/api/EntityStatusHistory?entityId={teamEntityId}").Result;

                statusHistory = JsonConvert.DeserializeObject<List<EntityStatusHistory>>(response);

                if (statusHistory.Count > 0)         
                {
                    return statusHistory.Where(x => x.status == "Like").Count();
                }
            }

            return 0;
        }


    }
}