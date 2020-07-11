using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsBook.Entities
{
    public class Stadium
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int MyProperty { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string StadiumImageFileName { get; set; }
    }
}
