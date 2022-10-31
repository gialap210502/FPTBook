using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using MvcFPTBook.Areas.Identity.Data;
using MvcFPTBook.Models;
using Microsoft.AspNetCore.Authorization;

namespace MvcFPTBook.Controllers
{
    public class PublishersController : Controller
    {
        private readonly MvcFPTBookIdentityDbContext _context;

        public PublishersController(MvcFPTBookIdentityDbContext context)
        {
            _context = context;
        }

        // GET: Publishers
        [Authorize(Roles = "Admin, StoreOwner")]
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber
        )
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentSort"] = sortOrder;

            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["AddressSortParm"] = sortOrder == "Address" ? "address_desc" : "Address";
            ViewData["CurrentFilter"] = searchString;

            var publishers = from p in _context.Publisher select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                publishers = publishers.Where(s => s.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    publishers = publishers.OrderByDescending(s => s.Name);
                    break;
                case "Address":
                    publishers = publishers.OrderBy(s => s.Address);
                    break;
                case "address_desc":
                    publishers = publishers.OrderByDescending(s => s.Phone);
                    break;
                default:
                    publishers = publishers.OrderBy(s => s.Name);
                    break;
            }
            int pageSize = 3;
            return View(
                await PaginatedList<Publisher>.CreateAsync(
                    publishers.AsNoTracking(),
                    pageNumber ?? 1,
                    pageSize
                )
            );
        }

        // GET: Publishers/Details/5
        [Authorize(Roles = "Admin, StoreOwner")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Publisher == null)
            {
                return NotFound();
            }

            var publisher = await _context.Publisher.FirstOrDefaultAsync(m => m.Id == id);
            if (publisher == null)
            {
                return NotFound();
            }

            return View(publisher);
        }

        // GET: Publishers/Create
        [Authorize(Roles = "Admin, StoreOwner")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Publishers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, StoreOwner")]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,Phone")] Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(publisher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(publisher);
        }

        // GET: Publishers/Edit/5
        [Authorize(Roles = "Admin, StoreOwner")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Publisher == null)
            {
                return NotFound();
            }

            var publisher = await _context.Publisher.FindAsync(id);
            if (publisher == null)
            {
                return NotFound();
            }
            return View(publisher);
        }

        // POST: Publishers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, StoreOwner")]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Name,Address,Phone")] Publisher publisher
        )
        {
            if (id != publisher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(publisher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PublisherExists(publisher.Id))
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
            return View(publisher);
        }

        // GET: Publishers/Delete/5
        [Authorize(Roles = "Admin, StoreOwner")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Publisher == null)
            {
                return NotFound();
            }

            var publisher = await _context.Publisher.FirstOrDefaultAsync(m => m.Id == id);
            if (publisher == null)
            {
                return NotFound();
            }

            return View(publisher);
        }

        // POST: Publishers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, StoreOwner")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Publisher == null)
            {
                return Problem("Entity set 'MvcBookContext.Publisher'  is null.");
            }
            var publisher = await _context.Publisher.FindAsync(id);
            if (publisher != null)
            {
                _context.Publisher.Remove(publisher);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PublisherExists(int id)
        {
            return (_context.Publisher?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [Authorize(Roles = "Admin, StoreOwner")]
        public IActionResult ExportPublicsherList()
        {
            //get data from database using EF
            List<Publisher> publishers = _context.Publisher.ToList<Publisher>();
            var stream = new MemoryStream();
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("Publishers");

                worksheet.Cells["A1"].Value = "Category List of FPT Book System";
                worksheet.Cells["A3"].Value = "ID";
                worksheet.Cells["B3"].Value = "Name";
                worksheet.Cells["C3"].Value = "Address";
                worksheet.Cells["D3"].Value = "Phone";

                int row = 4;
                foreach (var publisher in publishers)
                {
                    worksheet.Cells[row, 1].Value = publisher.Id;
                    worksheet.Cells[row, 2].Value = publisher.Name;
                    worksheet.Cells[row, 3].Value = publisher.Address;
                    worksheet.Cells[row, 4].Value = publisher.Phone;

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
    }
}
