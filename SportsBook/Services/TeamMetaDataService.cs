using System.Collections.Generic;
using SportsBook.Entities;
using SportsBook.Interfaces;

namespace SportsBook.Services
{
    public class TeamMetaDataService : ITeamMetaDataService
    {
        private readonly ITeamRepository _teamRepository;

        private readonly ICommentsRepository _commentsRepository;

        public TeamMetaDataService(ITeamRepository teamRepository, ICommentsRepository commentsRepository)
        {
            _teamRepository = teamRepository;
            _commentsRepository = commentsRepository;
        }      

        public List<Team> GetAllTeams()
        {
            List<Team> teams = _teamRepository.AllTeams;

            foreach(var team in teams) {
                List<EntityNote> entityNotes = _commentsRepository.GetComments(team.EntityId);
                team.NumberOfComments = entityNotes.Count;
            }

            return teams;
        }
    }
}