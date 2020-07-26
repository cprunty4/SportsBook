using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using SportsBook.Interfaces;
using SportsBook.Models;
using System.Linq;
using SportsBook.Entities;
using Newtonsoft.Json;
using System.Net.Http;

namespace SportsBook.Repository
{
    public class CommentsRepository : ICommentsRepository
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IConfiguration _configuration;

        private string baseUrl = string.Empty;    

        public CommentsRepository(ITeamRepository teamRepository, IConfiguration configuration)
        {
            _teamRepository = teamRepository;
            _configuration = configuration;
            baseUrl = _configuration.GetSection("EntitiesApiOptions").GetValue<string>("BaseUrl");            
        }

        public List<EntityNote> AllComments => throw new System.NotImplementedException();

        public List<EntityNote> GetComments(int teamEntityId)
        {
            List<EntityNote> entityNotes = new List<EntityNote>();
            // Call Service here that retrieves data from Entities API

            var client = new HttpClient();

            string response = client.GetStringAsync($"{baseUrl}/api/EntityNotes/{teamEntityId}").Result;

            entityNotes = JsonConvert.DeserializeObject<List<EntityNote>>(response);

            if (entityNotes.Count > 0)
            {
                return entityNotes;
            }


            return entityNotes;
        }

        public CommentsData GetCommentsData(int teamId)
        {
            CommentsData commentsData = new CommentsData();
            Team team = _teamRepository.AllTeams.Where(x => x.Id == teamId).First();

            commentsData.TeamName = team.Name;
            commentsData.TeamLocation = team.Location;
            commentsData.TeamHelmetImageFileName = team.HelmetImageFileName;
            commentsData.Comments = this.GetComments((int)team.EntityId);
            commentsData.NumberOfComments = commentsData.Comments.Count;

            

            return commentsData;

        }
    }
}