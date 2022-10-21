using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using MvcFPTBook.Models;
using MvcFPTBook.Areas.Identity.Data;
using MvcFPTBook.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace MvcFPTBook.Controllers
{
    public class BooksController : Controller
    {
        private readonly UserManager<BookUser> _userManager;

        private readonly MvcFPTBookIdentityDbContext _context;
        private readonly IWebHostEnvironment hostEnvironment;

        public BooksController(
            MvcFPTBookIdentityDbContext context,
            IWebHostEnvironment environment,
            UserManager<BookUser> userManager
        )
        {
            _context = context;

            _userManager = userManager;

            hostEnvironment = environment;
        }

        // GET: Books
        [Authorize(Roles = "Admin, StoreOwner")]
        public async Task<IActionResult> Index()
        {
            var mvcBookContext = _context.Book
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Publishers);
            return View(await mvcBookContext.ToListAsync());
        }

        // GET: Books/Details/5
        [Authorize(Roles = "Admin, StoreOwner")]
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
        [Authorize(Roles = "Admin, StoreOwner")]
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
        [Authorize(Roles = "Admin, StoreOwner")]
        public async Task<IActionResult> Create(
            [Bind("Id,Name,Poster,publiccationDate,Price,AuthorID,CategoryID,PublisherID")]
                Book book,
            IFormFile myfile
        )
        {
            if (!ModelState.IsValid)
            {
                string filename = Path.GetFileName(myfile.FileName);
                var filePath = Path.Combine(hostEnvironment.WebRootPath, "uploads");
                string fullPath = filePath + "\\" + filename;
                // Copy files to FileSystem using Streams
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await myfile.CopyToAsync(stream);
                }
                book.Poster = filename;
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorID"] = new SelectList(_context.Author, "Id", "Name", book.AuthorID);
            ViewData["CategoryID"] = new SelectList(
                _context.Category,
                "Id",
                "Name",
                book.CategoryID
            );
            ViewData["PublisherID"] = new SelectList(
                _context.Publisher,
                "Id",
                "Name",
                book.PublisherID
            );
            return View(book);
        }

        // GET: Books/Edit/5
        [Authorize(Roles = "Admin, StoreOwner")]
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
            ViewData["CategoryID"] = new SelectList(
                _context.Category,
                "Id",
                "Name",
                book.CategoryID
            );
            ViewData["PublisherID"] = new SelectList(
                _context.Publisher,
                "Id",
                "Name",
                book.PublisherID
            );
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, StoreOwner")]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Name,Poster,publiccationDate,Price,AuthorID,CategoryID,PublisherID")]
                Book book
        )
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
            ViewData["CategoryID"] = new SelectList(
                _context.Category,
                "Id",
                "Name",
                book.CategoryID
            );
            ViewData["PublisherID"] = new SelectList(
                _context.Publisher,
                "Id",
                "Name",
                book.PublisherID
            );
            return View(book);
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "Admin, StoreOwner")]
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
        [Authorize(Roles = "Admin, StoreOwner")]
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

        [HttpPost]
        public IActionResult AddTicket(int id, string name, decimal price, int quantity)
        {
            ShoppingCart myCart;
            // If the cart is not in the session, create one and put it there
            // Otherwise, get it from the session
            if (HttpContext.Session.GetObject<ShoppingCart>("cart") == null)
            {
                myCart = new ShoppingCart();
                HttpContext.Session.SetObject("cart", myCart);
            }
            myCart = (ShoppingCart)HttpContext.Session.GetObject<ShoppingCart>("cart");
            var newItem = myCart.AddItem(id, name, price, quantity);
            HttpContext.Session.SetObject("cart", myCart);
            ViewData["newItem"] = newItem;
            return View();
        }

        public IActionResult CheckOut()
        {
            ShoppingCart myCart;
            if (HttpContext.Session.GetObject<ShoppingCart>("cart") == null)
            {
                myCart = new ShoppingCart();
                HttpContext.Session.SetObject("cart", myCart);
            }
            myCart = (ShoppingCart)HttpContext.Session.GetObject<ShoppingCart>("cart");
            ViewData["myItems"] = myCart.Items;
            return View();
        }

        public IActionResult PlaceOrder(decimal total)
        {
            ShoppingCart cart = (ShoppingCart)HttpContext.Session.GetObject<ShoppingCart>("cart");
            Order myOrder = new Order();
            myOrder.OrderTime = DateTime.Now;
            myOrder.Total = total;
            var userid = _userManager.GetUserId(HttpContext.User);
            BookUser user = _userManager.FindByIdAsync(userid).Result;
            if (user == null)
            {
                return RedirectToAction("Account/Login", "Identity");
            }
            myOrder.BookUserId = user.Id;

            _context.Order.Add(myOrder);
            _context.SaveChanges(); //this generates the Id for Order

            foreach (var item in cart.Items)
            {
                OrderDetail myOrderItem = new OrderDetail();
                myOrderItem.BookId = item.ID;
                myOrderItem.Quantity = item.Quantity;
                myOrderItem.OrderId = myOrder.Id; //id of saved order above

                _context.OrderDetail.Add(myOrderItem);
            }
            //_context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[OrderDetail] ON");
            _context.SaveChanges();
            //empty shopping cart
            cart = new ShoppingCart();
            HttpContext.Session.SetObject("cart", cart);
            return View();
        }

        [HttpPost]
        public RedirectToActionResult EditOrder(int id, int quantity)
        {
            ShoppingCart cart = (ShoppingCart)HttpContext.Session.GetObject<ShoppingCart>("cart");
            cart.EditItem(id, quantity);
            HttpContext.Session.SetObject("cart", cart);

            return RedirectToAction("CheckOut", "Books");
        }

        [HttpPost]
        public RedirectToActionResult RemoveOrderItem(int id)
        {
            ShoppingCart cart = (ShoppingCart)HttpContext.Session.GetObject<ShoppingCart>("cart");
            cart.RemoveItem(id);
            HttpContext.Session.SetObject("cart", cart);

            return RedirectToAction("CheckOut", "Books");
        }

        [Authorize(Roles = "Admin, StoreOwner")]
        public IActionResult ExportBookList()
        {
            //get data from database using EF
            List<Book> books = _context.Book.ToList<Book>();
            var stream = new MemoryStream();
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("Books");

                worksheet.Cells["A1"].Value = "Book List of FPT Book System";
                worksheet.Cells["A3"].Value = "ID";
                worksheet.Cells["B3"].Value = "Name";
                worksheet.Cells["C3"].Value = "Publiccation Date";
                worksheet.Cells["D3"].Value = "Price";
                worksheet.Cells["F3"].Value = "Author";
                worksheet.Cells["G3"].Value = "Category";
                worksheet.Cells["H3"].Value = "Publisher";

                int row = 4;
                foreach (var book in books)
                {
                    worksheet.Cells[row, 1].Value = book.Id;
                    worksheet.Cells[row, 2].Value = book.Name;
                    worksheet.Cells[row, 3].Value = book.publiccationDate;
                    worksheet.Cells[row, 4].Value = book.Price;
                    worksheet.Cells[row, 5].Value = book.Author.Name;
                    worksheet.Cells[row, 6].Value = book.Category.Name;
                    worksheet.Cells[row, 7].Value = book.Publishers.Name;

                    row++;
                }
                xlPackage.Save();
            }
            stream.Position = 0;
            return File(
                stream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "genres.xlsx"
            );
        }

        public IActionResult ReportDemo()
        {
            var data = _context.OrderDetail
                .Include(s => s.Book)
                .GroupBy(s => s.Book.Name)
                .Select(g => new { Name = g.Key, Total = g.Sum(s => s.Quantity) })
                .ToList();

            string[] labels = new string[data.Count];
            string[] totals = new string[data.Count];

            for (int i = 0; i < data.Count; i++)
            {
                labels[i] = data[i].Name;
                totals[i] = data[i].Total.ToString();
            }

            ViewData["labels"] = string.Format("'{0}'", String.Join("','", labels));
            ViewData["totals"] = String.Join(",", totals);

            return View(data);
        }
    }
}
