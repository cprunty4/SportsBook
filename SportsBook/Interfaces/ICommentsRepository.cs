using System.Collections.Generic;
using SportsBook.Models;

namespace SportsBook.Interfaces
{
    public interface ICommentsRepository
    {
         List<EntityNote> AllComments {get;}

         List<EntityNote> GetComments(int teamEntityId);

         CommentsData GetCommentsData(int teamId);
    }
}