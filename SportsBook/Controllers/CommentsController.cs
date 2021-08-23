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

        private readonly IAzureBlobService _azureBlobService;

        public CommentsController(ILogger<CommentsController> logger,
            ICommentsRepository commentsRepository,
            IAzureBlobService azureBlobService,
        )
        {
            _logger = logger;
            _commentsRepository = commentsRepository;
            _azureBlobService = azureBlobService;
        }

        public IActionResult Index(int teamId)
        {
            _logger.LogInformation("entered Comments controller");
            CommentsData commentsData = this._commentsRepository.GetCommentsData(teamId);

            commentsData.LogoImage = _azureBlobService.GetImageUri(commentsData.LogoImage);

            return View(commentsData);
        }

    }
}