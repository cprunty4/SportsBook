using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
                    if (filteredGameSlates.Count > 0)   
                        filteredGameSlates.AddRange(gameSlates.Where(x => x.AwayTeamFullName != null
                        && x.AwayTeamFullName.ToUpper().Contains(request.teamName.ToUpper())).ToList());
                    else
                        filteredGameSlates = gameSlates.Where(x => x.AwayTeamFullName != null
                        && x.AwayTeamFullName.ToUpper().Contains(request.teamName.ToUpper())).ToList();

                }

                if (filteredGameSlates.Count == 0)
                {
                    gameSlates = new List<GameSlate>();
                }

            }

            // check for Start/End date filtering
            if (request.startDate != System.DateTime.MinValue
                && request.endDate != System.DateTime.MinValue)
            {
                if (filteredGameSlates.Count > 0)                   
                    filteredGameSlates = filteredGameSlates.Where(x => 
                    x.GameStartDateTime != null
                    && x.GameStartDateTime >= request.startDate
                    && x.GameStartDateTime <= request.endDate
                    ).ToList();
                else
                    filteredGameSlates = gameSlates.Where(x => 
                    x.GameStartDateTime != null
                    && x.GameStartDateTime >= request.startDate
                    && x.GameStartDateTime <= request.endDate
                    ).ToList();  

                if (filteredGameSlates.Count == 0)
                {
                    gameSlates = new List<GameSlate>();
                }                                      

            }

            if (filteredGameSlates.Count > 0)
                gameSlates = filteredGameSlates;

            // Order by Game date/time
            gameSlates = gameSlates.OrderBy(x => x.GameStartDateTime).ToList();

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

        public GamesSearchResponse GetGamesSearchApi(GamesSearchRequest request)
        {
            List<GameSlate> gameSlates = new List<GameSlate>();
            // Call Service here that retrieves data from Entities API

            var client = new HttpClient();

            var requestContent = JsonConvert.SerializeObject(request);

            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{baseUrl}/api/GamesSearch"),
                Content = new StringContent(requestContent.ToString(), Encoding.UTF8, "application/json"),
            };

            // Add ApiKey header
            client.DefaultRequestHeaders.Add("ApiKey", "BackendAdmin2021");
            
            var httpResponse = client.SendAsync(httpRequest).Result;

            var responseContent = httpResponse.Content.ReadAsStringAsync().Result;

            //Map ML Response from the BCResponse
            GamesSearchResponse response = JsonConvert.DeserializeObject<GamesSearchResponse>(responseContent);

            return response;
        }
    }
}