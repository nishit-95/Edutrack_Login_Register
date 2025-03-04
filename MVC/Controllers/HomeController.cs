using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;

namespace MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult RegisterForm()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }
    public IActionResult StuDash()
    {
        return View();
    }
    public IActionResult Payment()
    {
        return View();
    }
    public async Task<ActionResult> Logout()
    {
        return RedirectToAction("Index", "Home");
    }

     public IActionResult Feedback()
    {
        return View();
    }
     public IActionResult Timetable()
    {
        return View();
    }

    public IActionResult UpdateTeacher()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
