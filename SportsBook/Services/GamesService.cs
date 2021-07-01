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
        IGameSlateRepository gameSlateRepository) {
            _configuration = configuration;
            baseUrl = _configuration.GetSection("EntitiesApiOptions").GetValue<string>("BaseUrl");             
            _gameSlateRepository = gameSlateRepository;            
        }
        public GamesSearchResponse GetGamesSearch(GamesSearchRequest request)
        {
            GamesSearchResponse gamesSearchResponse = new GamesSearchResponse();
            List<GameSlate> gameSlates = _gameSlateRepository.AllGameSlates;

            PagingOptionsModel pagingOptions = new PagingOptionsModel();
            if (request.pagingOptions != null)
                pagingOptions = request.pagingOptions;

            // Implement paging
            var pagedGameSlates = gameSlates.Skip(pagingOptions.PageSize * (pagingOptions.Page - 1))
                                        .Take(pagingOptions.PageSize).ToList();

            // add paging response
            gamesSearchResponse.pagedResult = new PagedResultModel
            {
                TotalResultCount = gameSlates.Count,
                PagingOptions = pagingOptions,
                HasMoreResults = gameSlates.Count > pagingOptions.Page * pagingOptions.PageSize
            };

            gamesSearchResponse.GameSlates = pagedGameSlates;                

            return gamesSearchResponse;
        }
    }
}