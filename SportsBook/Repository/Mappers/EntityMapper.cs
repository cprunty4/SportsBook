using System;
using System.Collections.Generic;
using System.Linq;
using SportsBook.Entities;

namespace SportsBook.Repository.Mappers
{
    public static class EntityMapper
    {
        internal static List<Team> MapEntityToTeams(List<Entity> entities)
        {
            List<Team> teams = new List<Team>();

            foreach(var entity in entities.OrderByDescending(x => x.UpdatedDate))
            {
                teams.Add(new Team {
                    FullName = entity.Name,
                    LogoImage = entity.Description,
                    EntityId = (int)entity.ID
                });
            }

            return teams;
        }

        internal static Team MapEntityToTeam(Entity entity)
        {
            return new Team {
                FullName = entity.Name,
                LogoImage = entity.Description,
                EntityId = (int)entity.ID
            };
        }

        internal static List<Stadium> MapEntityToStadiums(List<Entity> entities)
        {
            List<Stadium> stadiums = new List<Stadium>();

            foreach (var entity in entities.OrderByDescending(x => x.UpdatedDate))
            {
                stadiums.Add(new Stadium
                {
                    Name = entity.Name,
                    StadiumImageFileName = entity.Description,
                    Id = (int)entity.ID
                });
            }

            return stadiums;
        }

        internal static Stadium MapEntityToStadium(Entity entity)
        {
            return new Stadium
            {
                Name = entity.Name,
                StadiumImageFileName = entity.Description,
                Id = (int)entity.ID
            };
        }
    }
}