using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using web.Data;
using web.Models;

namespace web.Controllers;

public class HomeController : Controller {
    private readonly ILogger<HomeController> _logger;
    private readonly RentContext _context;

    public HomeController(ILogger<HomeController> logger, RentContext context) {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index() {
        var cars = _context.Cars.ToList();
        
        return View(cars);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
