using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsBook.Entities;

namespace SportsBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        public TeamController()
        {
        }

        // GET api/team
        [HttpGet("")]
        public ActionResult<IEnumerable<string>> Getstrings()
        {
            return new List<string> { };
        }

        // GET api/team/5
        [HttpGet("{id}")]
        public ActionResult<string> GetstringById(int id)
        {
            return null;
        }

        // POST api/team
        [HttpPost("")]
        public void Poststring(string value)
        {
        }

        // PUT api/team/5
        [HttpPut("{id}")]
        public void Putstring(int id, string value)
        {
        }

        // DELETE api/team/5
        [HttpDelete("{id}")]
        public void DeletestringById(int id)
        {
        }
    }
}