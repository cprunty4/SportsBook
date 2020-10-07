using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SportsBook.Entities;
using SportsBook.Interfaces;
using SportsBook.Models;

namespace SportsBook.Repository
{
    public class TeamMetaDataRepository : ITeamMetaDataRepository
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IConfiguration _configuration;
        private readonly ICommentsRepository _commentsRepository;

        private string baseUrl = string.Empty;        

        public TeamMetaDataRepository(ITeamRepository teamRepository,
            IConfiguration configuration,
            ICommentsRepository commentsRepository
            )
        {
            _teamRepository = teamRepository;
            _configuration = configuration;
            baseUrl = _configuration.GetSection("EntitiesApiOptions").GetValue<string>("BaseUrl");
            _commentsRepository = commentsRepository;
        }        
        public List<TeamMetaData> AllTeamsMetaData => this.FetchAllTeamsMetaData();

        private List<TeamMetaData> FetchAllTeamsMetaData()
        {
            List<TeamMetaData> teamsMetaData = new List<TeamMetaData>();

            var teams = _teamRepository.AllTeams;

            foreach(var team in teams)            
            {
                var teamMetaDate = new TeamMetaData();

                teamMetaDate.TeamId = team.Id;
                teamMetaDate.TeamLocation = team.LocationName;
                teamMetaDate.TeamName = team.NickName;
                teamMetaDate.LogoImage = team.LogoImage;
                List<EntityNote> comments = _commentsRepository.GetComments(team.EntityId);
                // teamMetaDate.NumberOfComments = this.GetNumberOfComments(team.EntityId);
                teamMetaDate.NumberOfComments = comments.Count;
                if (comments.Count > 0)
                    teamMetaDate.LastCommentUpdatedDate = (from c in comments select c.UpdatedDate).Max();
                teamMetaDate.NumberOfLikes = this.GetNumberOfLikes(team.EntityId);
                teamsMetaData.Add(teamMetaDate);
            }

            return teamsMetaData.Select(x => x).OrderByDescending(x => x.LastCommentUpdatedDate).ToList();
        }

        internal int GetNumberOfComments(int? teamEntityId)
        {
            List<EntityNote> entityNotes = new List<EntityNote>();
            // Call Service here that retrieves data from Entities API

            if (teamEntityId != null)
            {
                var client = new HttpClient();

                string response = client.GetStringAsync($"{baseUrl}/api/EntityNotes/{teamEntityId}").Result;

                entityNotes = JsonConvert.DeserializeObject<List<EntityNote>>(response);

                if (entityNotes.Count > 0)
                {
                    return entityNotes.Count;
                }
            }

            return 0;
        }

        internal int GetNumberOfLikes(int? teamEntityId) {
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