using SportsBook.Interfaces;
using SportsBook.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsBook.Repository
{
    public class MockStadiumRepository : IStadiumRepository
    {
        public List<Stadium> AllStadiums =>
            new List<Stadium>
            {
                new Stadium
                {
                    Address = "1265 Lombardi Ave",
                    City = "Green Bay",
                    Id = 1,
                    Name = "Lambeau Field",
                    State = "WI",
                    ZipCode = "54304"
                },
                new Stadium
                {
                    Address = "1 Arrowhead Dr",
                    City = "Kansas City",
                    Id = 2,
                    Name = "Arrowhead Stadium",
                    State = "MO",
                    ZipCode = "64129"
                },
                new Stadium
                {
                    Address = "1 AT&T Way",
                    City = "Arlington",
                    Id = 3,
                    Name = "AT&T Stadium",
                    State = "TX",
                    ZipCode = "76011"
                },
                new Stadium {
                    Address = "401 Chicago Ave",
                    City = "Minneapolis",
                    Id = 4,
                    Name = "U.S. Bank Stadium",
                    State = "MN",
                    ZipCode = "55415"

                }
            };

        public Stadium GetStadiumById(long stadiumId)
        {
            return this.AllStadiums.Where(x => x.Id == stadiumId).FirstOrDefault();
        }
    }
}
