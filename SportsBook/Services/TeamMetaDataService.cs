using System.Collections.Generic;
using SportsBook.Entities;
using SportsBook.Interfaces;

namespace SportsBook.Services
{
    public class TeamMetaDataService : ITeamMetaDataService
    {
        private readonly ITeamRepository _teamRepository;

        private readonly ICommentsRepository _commentsRepository;

        private readonly IAzureBlobService _azureBlobService;

        public TeamMetaDataService(ITeamRepository teamRepository, ICommentsRepository commentsRepository, IAzureBlobService azureBlobService)
        {
            _teamRepository = teamRepository;
            _commentsRepository = commentsRepository;
            _azureBlobService = azureBlobService;
        }      

        public List<Team> GetAllTeams()
        {
            List<Team> teams = _teamRepository.AllTeams;

            foreach(var team in teams) {
                List<EntityNote> entityNotes = _commentsRepository.GetComments(team.EntityId);
                team.NumberOfComments = entityNotes.Count;

                int numberOfLikes = _commentsRepository.GetNumberOfLikes(team.EntityId);
                team.NumberOfLikes = numberOfLikes;

                // Retrieve the LogoImage from Azure Storage
                team.LogoImageUri = _azureBlobService.GetImageUri(team.LogoImage);

            }

            return teams;
        }
    }
}