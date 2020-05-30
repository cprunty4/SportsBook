using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SportsBook.Interfaces;

public class TeamController : Controller
{
    private readonly ILogger<TeamController> _logger;
    private readonly ITeamRepository _teamRepository;
    public TeamController(ILogger<TeamController> logger, ITeamRepository teamRepository)
    {
        _logger = logger;
        _teamRepository = teamRepository;
    }

    public IActionResult Index()
    {
        return View();
    }
}