using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SportsBook.Interfaces;
using SportsBook.Models;

namespace SportsBook.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ILogger<CommentsController> _logger;
        private readonly ICommentsRepository _commentsRepository;
        public CommentsController(ILogger<CommentsController> logger,
            ICommentsRepository commentsRepository
        
        )
        {
            _logger = logger;
            _commentsRepository = commentsRepository;
        }

        public IActionResult Index(int teamId)
        {
            _logger.LogInformation("entered Comments controller");
            CommentsData commentsData = this._commentsRepository.GetCommentsData(teamId);
            return View(commentsData);
        }        

    }
}