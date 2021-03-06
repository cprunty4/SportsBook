﻿using SportsBook.Interfaces;
using SportsBook.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsBook.Repository
{
    public class MockTeamRepository : ITeamRepository
    {
        public List<Team> AllTeams => 
        
            new List<Team>
            {
                new Team
                {
                    Id = 1,
                    LocationName = "Green Bay",
                    TeamAbbreviation = "GBP",
                    NickName = "Packers",
                    Conference = "NFC",
                    Division = "NFC North",
                    Colors = new List<string> {"DarkGreen", "Gold"},
                    HomeStadiumId = 1,
                    LogoImage = "Green_Bay_Packers_logo.png",
                    EntityId =1
                },
                new Team
                {
                    Id = 2,
                    LocationName = "Kansas City",
                    TeamAbbreviation = "KC",
                    NickName = "Chiefs",
                    Conference = "AFC",
                    Division = "AFC West",
                    Colors = new List<string> {"Red", "Gold"},
                    HomeStadiumId = 2,
                    LogoImage = "Kansas_City_Chiefs_logo.svg",
                    EntityId=2
                },
                new Team
                {
                    Id = 3,
                    LocationName = "Dallas",
                    TeamAbbreviation = "DAL",
                    NickName = "Cowboys",
                    Conference = "NFC",
                    Division = "NFC East",
                    Colors = new List<string> {"NavyBlue", "Silver", "RoyalBlue"},
                    LogoImage = "Dallas_Cowboys.png",
                    EntityId = 3
                },
                new Team
                {
                    Id = 4,
                    LocationName = "Minnesota",
                    TeamAbbreviation = "MIN",
                    NickName = "Vikings",
                    Conference = "NFC",
                    Division = "NFC North",
                    Colors = new List<string> {"Purple", "Gold"},
                    HomeStadiumId = 4,
                    LogoImage = "Minnesota_Vikings_logo.png",
                    EntityId = 4
                },
                new Team
                {
                    Id = 5,
                    LocationName = "Seattle",
                    TeamAbbreviation = "SEA",
                    NickName = "Seahawks",
                    Conference = "NFC",
                    Division = "NFC West",
                    Colors = new List<string> {"College Navy", "Action Green", "Wolf Grey"},
                    LogoImage = "Seattle_Seahawks_logo.png",
                    EntityId = 5
                },
                new Team
                {
                    Id = 6,
                    LocationName = "Tennessee",
                    TeamAbbreviation = "TEN",
                    NickName = "Titans",
                    Conference = "AFC",
                    Division = "AFC South",
                    Colors = new List<string> {"Navy", "Titans Blue", "Red", "Silver", "White"},
                    LogoImage = "Tennessee_Titans_logo.svg.png",
                    EntityId = 6
                },
                new Team
                {
                    Id = 7,
                    LocationName = "New England",
                    TeamAbbreviation = "NE",
                    NickName = "Patriots",
                    Conference = "AFC",
                    Division = "AFC East",
                    Colors = new List<string> {"Navy Blue", "Red", "Silver", "White"},
                    LogoImage = "New_England_Patriots_logo.svg.png",
                    EntityId = 7
                },
                new Team
                {
                    Id = 8,
                    LocationName = "Indianapolis",
                    TeamAbbreviation = "IND",
                    NickName = "Colts",
                    Conference = "AFC",
                    Division = "AFC South",
                    Colors = new List<string> {"Speed Blue", "White", "Facemask Gray", "Black"},
                    LogoImage = "Indianapolis_Colts_logo.svg.png",
                    EntityId = 8
                },
                new Team
                {
                    Id = 9,
                    LocationName = "Los Angeles",
                    TeamAbbreviation = "LAC",
                    NickName = "Chargers",
                    Conference = "AFC",
                    Division = "AFC West",
                    Colors = new List<string> {"Powder Blue", "Sunshine Gold", "White"},
                    LogoImage = "Los_Angeles_Chargers_logo.svg.png",
                    EntityId = 9
                },
                new Team
                {
                    Id = 10,
                    LocationName = "Cincinnati",
                    TeamAbbreviation = "CIN",
                    NickName = "Bengals",
                    Conference = "AFC",
                    Division = "AFC North",
                    Colors = new List<string> {"Black", "Orange", "White"},
                    LogoImage = "Cincinnati_Bengals_logo.svg.png",
                    EntityId = 10
                },
                new Team
                {
                    Id = 11,
                    LocationName = "San Francisco",
                    TeamAbbreviation = "SF",
                    NickName = "49ers",
                    Conference = "NFC",
                    Division = "NFC West",
                    Colors = new List<string> {"Red", "Gold"},
                    LogoImage = "San_Francisco_49ers_logo.svg.png",
                    EntityId = 11
                },
                new Team
                {
                    Id = 12,
                    LocationName = "Arizona",
                    TeamAbbreviation = "ARI",
                    NickName = "Cardinals",
                    Conference = "NFC",
                    Division = "NFC West",
                    Colors = new List<string> {"Cardinal Red", "White", "Black"},
                    LogoImage = "Arizona_Cardinals_logo.svg.png",
                    EntityId = 12
                },
                new Team
                {
                    Id = 13,
                    LocationName = "New Orleans",
                    TeamAbbreviation = "NO",
                    NickName = "Saints",
                    Conference = "NFC",
                    Division = "NFC South",
                    Colors = new List<string> {"Old Gold", "black", "white"},
                    LogoImage = "Orleans_Saints_logo.svg.png",
                    EntityId = 13
                },
                new Team
                {
                    Id = 14,
                    LocationName = "Detroit",
                    TeamAbbreviation = "DET",
                    NickName = "Lions",
                    Conference = "NFC",
                    Division = "NFC Central",
                    Colors = new List<string> {"Honolulu Blue", "Silver"},
                    LogoImage = "Detroit_Lions_logo.svg.png",
                    EntityId = 14
                }                                 

            };


        public Team GetTeamById(long teamId)
        {
            return this.AllTeams.Where(x => x.Id == teamId).FirstOrDefault();
        }
    }
}
