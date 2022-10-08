using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using MvcFPTBook.Models;
using MvcFPTBook.Data;

namespace MvcFPTBook.Controllers
{
    public class BooksController : Controller
    {
        private readonly MvcBookContext _context;
        private readonly IWebHostEnvironment hostEnvironment;

        public BooksController(MvcBookContext context,IWebHostEnvironment environment)
        {
            _context = context;
            hostEnvironment = environment;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var mvcBookContext = _context.Book.Include(b => b.Author).Include(b => b.Category).Include(b => b.Publishers);
            return View(await mvcBookContext.ToListAsync());
        }

        // GET: Books/Details/5
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

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["AuthorID"] = new SelectList(_context.Author, "Id", "Name");
            ViewData["CategoryID"] = new SelectList(_context.Category, "Id", "Name");
            ViewData["PublisherID"] = new SelectList(_context.Publisher, "Id", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Poster,publiccationDate,Price,AuthorID,CategoryID,PublisherID")] Book book,IFormFile myfile)
        {
            if (!ModelState.IsValid)
            {
                string filename=Path.GetFileName(myfile.FileName);
                var filePath = Path.Combine(hostEnvironment.WebRootPath, "uploads");
                string fullPath=filePath+"\\"+filename;
                // Copy files to FileSystem using Streams
                using (var stream = new FileStream(fullPath, FileMode.Create))
                    {	
                    await myfile.CopyToAsync(stream);
                    }
                book.Poster=filename;
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorID"] = new SelectList(_context.Author, "Id", "Name", book.AuthorID);
            ViewData["CategoryID"] = new SelectList(_context.Category, "Id", "Name", book.CategoryID);
            ViewData["PublisherID"] = new SelectList(_context.Publisher, "Id", "Name", book.PublisherID);
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["AuthorID"] = new SelectList(_context.Author, "Id", "Name", book.AuthorID);
            ViewData["CategoryID"] = new SelectList(_context.Category, "Id", "Name", book.CategoryID);
            ViewData["PublisherID"] = new SelectList(_context.Publisher, "Id", "Name", book.PublisherID);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Poster,publiccationDate,Price,AuthorID,CategoryID,PublisherID")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorID"] = new SelectList(_context.Author, "Id", "Name", book.AuthorID);
            ViewData["CategoryID"] = new SelectList(_context.Category, "Id", "Name", book.CategoryID);
            ViewData["PublisherID"] = new SelectList(_context.Publisher, "Id", "Name", book.PublisherID);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Book == null)
            {
                return Problem("Entity set 'MvcBookContext.Book'  is null.");
            }
            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                _context.Book.Remove(book);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
          return (_context.Book?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
