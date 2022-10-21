using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MvcFPTBook.Models;
using MvcFPTBook.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace MvcFPTBook.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly MvcFPTBookIdentityDbContext _context;

    public HomeController(ILogger<HomeController> logger, MvcFPTBookIdentityDbContext context)
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
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Book == null)
        {
            return NotFound();
        }

        var book = await _context.Book
            .Include(b => b.Author)
            .Include(b => b.Category)
            .Include(b => b.Publishers)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }
}
