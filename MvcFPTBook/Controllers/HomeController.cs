using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MvcFPTBook.Models;
using MvcFPTBook.Data;
using Microsoft.EntityFrameworkCore;

namespace MvcFPTBook.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly MvcBookContext _context;

    public HomeController(ILogger<HomeController> logger, MvcBookContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var mvcBookContext = _context.Book.Include(b => b.Author).Include(b => b.Category).Include(b => b.Publishers);
        return View(await mvcBookContext.ToListAsync());
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
