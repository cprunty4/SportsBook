using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SportsBook.Interfaces;
using SportsBook.Models;

namespace SportsBook.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly ILogger<GameController> _logger;
        private readonly IGameRepository _gameRepository;
        public GameController(ILogger<GameController> logger, IGameRepository gameRepository)
        {
            _logger = logger;
            _gameRepository = gameRepository;
        }

        // GET api/game
        [HttpGet("")]
        public ActionResult<IEnumerable<string>> Getstrings()
        {
            return new List<string> { "this is a test" };
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