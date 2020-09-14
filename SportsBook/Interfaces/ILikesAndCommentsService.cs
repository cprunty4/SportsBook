namespace SportsBook.Interfaces
{
    public interface ILikesAndCommentsService
    {
         int GetNumberOfComments(int? teamEntityId);
         int GetNumberOfLikes(int? teamEntityId);
    }
}