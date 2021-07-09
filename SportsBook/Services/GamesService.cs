using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using SportsBook.Interfaces;
using SportsBook.Models;

namespace SportsBook.Services
{
    public class GamesService : IGamesService
    {
        private string baseUrl = string.Empty;
        private readonly IConfiguration _configuration;

        IGameSlateRepository _gameSlateRepository;
        public GamesService(IConfiguration configuration,
        IGameSlateRepository gameSlateRepository)
        {
            _configuration = configuration;
            baseUrl = _configuration.GetSection("EntitiesApiOptions").GetValue<string>("BaseUrl");
            _gameSlateRepository = gameSlateRepository;
        }
        public GamesSearchResponse GetGamesSearch(GamesSearchRequest request)
        {
            GamesSearchResponse gamesSearchResponse = new GamesSearchResponse();
            List<GameSlate> gameSlates = _gameSlateRepository.AllGameSlates;
            List<GameSlate> filteredGameSlates = new List<GameSlate>();
            filteredGameSlates = gameSlates;
            // Implement teamName filtering
            if (!string.IsNullOrEmpty(request.teamName))
            {
                if (gameSlates.Any(x => x.HomeTeamFullName != null
                && x.HomeTeamFullName.ToUpper().Contains(request.teamName.ToUpper())))
                {
                    filteredGameSlates = gameSlates.Where(x => x.HomeTeamFullName != null
                    && x.HomeTeamFullName.ToUpper().Contains(request.teamName.ToUpper())).ToList();
                }
                
                if (gameSlates.Any(x => x.AwayTeamFullName != null
                && x.AwayTeamFullName.ToUpper().Contains(request.teamName.ToUpper())))
                {   
                    filteredGameSlates.AddRange(gameSlates.Where(x => x.AwayTeamFullName != null
                    && x.AwayTeamFullName.ToUpper().Contains(request.teamName.ToUpper())).ToList());

                }
            }

            // check for Start/End date filtering
            if (request.startDate != System.DateTime.MinValue
                && request.endDate != System.DateTime.MinValue)
            {
                filteredGameSlates = filteredGameSlates.Where(x => 
                x.GameStartDateTime != null
                && x.GameStartDateTime >= request.startDate
                && x.GameStartDateTime <= request.endDate
                ).ToList();

            }

            PagingOptionsModel pagingOptions = new PagingOptionsModel();
            if (request.pagingOptions != null)
                pagingOptions = request.pagingOptions;

            // Implement paging
            var pagedGameSlates = filteredGameSlates.Skip(pagingOptions.PageSize * (pagingOptions.Page - 1))
                                        .Take(pagingOptions.PageSize).ToList();

            // add paging response
            gamesSearchResponse.pagedResult = new PagedResultModel
            {
                TotalResultCount = filteredGameSlates.Count,
                PagingOptions = pagingOptions,
                HasMoreResults = filteredGameSlates.Count > pagingOptions.Page * pagingOptions.PageSize
            };

            gamesSearchResponse.GameSlates = pagedGameSlates;

            return gamesSearchResponse;
        }
    }
}