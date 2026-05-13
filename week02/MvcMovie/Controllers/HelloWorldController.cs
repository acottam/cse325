using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace MvcMovie.Controllers;

public class HelloWorldController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Welcome(string name, int ID = 1)
    {
        ViewData["Message"] = $"Hello {name}";
        ViewData["NumTimes"] = ID;
        return View();
    }
}
