using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SportsBook.Entities;
using SportsBook.Models;

namespace SportsBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameTeamController : ControllerBase
    {
        private readonly ILogger<GameTeamController> _logger;
        public GameTeamController(ILogger<GameTeamController> logger)
        {
            _logger = logger;
        }

        // GET api/gameteam
        [HttpGet("")]
        public ActionResult<IEnumerable<string>> Getstrings()
        {
            return new List<string> { };
        }

        // GET api/gameteam/5
        [HttpGet("{id}")]
        public ActionResult<string> GetstringById(int id)
        {
            return null;
        }

        // POST api/gameteam
        [HttpPost("")]
        public void Poststring(string value)
        {
        }

        // PUT api/gameteam/5
        [HttpPut("{id}")]
        public void Putstring(int id, string value)
        {
        }

        // DELETE api/gameteam/5
        [HttpDelete("{id}")]
        public void DeletestringById(int id)
        {
        }
    }
}