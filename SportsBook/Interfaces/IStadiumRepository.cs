﻿using SportsBook.Models;
using SportsBook.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsBook.Interfaces
{
    public interface IStadiumRepository
    {
        List<Stadium> AllStadiums { get;}
        Stadium GetStadiumById(long stadiumId);
    }
}
