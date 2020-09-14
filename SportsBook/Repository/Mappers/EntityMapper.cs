using System;
using System.Collections.Generic;
using SportsBook.Entities;

namespace SportsBook.Repository.Mappers
{
    public static class EntityMapper
    {
        internal static List<Team> MapEntityToTeams(List<Entity> entities)
        {
            List<Team> teams = new List<Team>();

            foreach(var entity in entities)
            {
                teams.Add(new Team {
                    FullName = entity.Name,
                    LogoImage = entity.Description
                });
            }

            return teams;
        }
    }
}