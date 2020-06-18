using System;
using System.Collections.Generic;
using SportsBook.Interfaces;
using SportsBook.Models.Views;

namespace SportsBook.Repository
{
    public class MockTeamMetaDataRepository : ITeamMetaDataRepository
    {
        private readonly ITeamRepository _teamRepository;

        public MockTeamMetaDataRepository(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
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
                teamMetaDate.NumberOfLikes = this.GetNumberOfLikes(team.Id);

                teamsMetaData.Add(teamMetaDate);
            }


            return teamsMetaData;
        }

        private int GetNumberOfComments(long id)
        {

            // TODO Call Service here that retrieves data from Entities API
            return 0;
        }

        private int GetNumberOfLikes(long teamId) {
            // TODO Call Service here that retrieves data from Entities API            
            return 0;
        }


    }
}