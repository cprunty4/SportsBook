using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SportsBook.Interfaces;
using SportsBook.Models;
using SportsBook.Models.Views;

namespace SportsBook.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameSlateController : ControllerBase
    {
        private readonly ILogger<GameSlateController> _logger;
        private readonly IGameSlateRepository _gameRepository;
        public GameSlateController(ILogger<GameSlateController> logger, IGameSlateRepository gameRepository)
        {
            _logger = logger;
            _gameRepository = gameRepository;
        }

        // GET api/game
        [HttpGet("")]
        public ActionResult<IEnumerable<GameSlate>> Getstrings()
        {
            return _gameRepository.AllGameSlates;
        }

        // GET api/game/5
        [HttpGet("{id}")]
        public ActionResult<string> GetstringById(int id)
        {
            return null;
        }

        // POST api/game
        [HttpPost("")]
        public void Poststring(string value)
        {
        }

        // PUT api/game/5
        [HttpPut("{id}")]
        public void Putstring(int id, string value)
        {
        }

        // DELETE api/game/5
        [HttpDelete("{id}")]
        public void DeletestringById(int id)
        {
        }
    }
}