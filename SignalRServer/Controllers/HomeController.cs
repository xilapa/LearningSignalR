using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRServer.Hubs;
using SignalRServer.Models;

namespace SignalRServer.Controllers;

public class HomeController : Controller
{
    private readonly IHubContext<LearningHub, ILearningHubClient> _hubContext;

    public HomeController(IHubContext<LearningHub, ILearningHubClient> hubContext)
    {
        _hubContext = hubContext;
    }

    public IActionResult Index()
    {
        _hubContext.Clients.All.ReceiveMessage("Someone accessed the home page");
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
