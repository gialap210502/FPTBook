using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcFPTBook.Areas.Identity.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using MvcFPTBook.Models;
using Microsoft.AspNetCore.Authorization;

namespace MvcFPTBook.Controllers
{
    public class CategorysController : Controller
    {
        private readonly MvcFPTBookIdentityDbContext _context;

        public CategorysController(MvcFPTBookIdentityDbContext context)
        {
            _context = context;
        }

        // GET: Categorys
        [Authorize(Roles = "Admin, StoreOwner")]
        public async Task<IActionResult> Index()
        {
            return _context.Category != null
                ? View(await _context.Category.ToListAsync())
                : Problem("Entity set 'MvcBookContext.Category'  is null.");
        }

        // GET: Categorys/Details/5
        [Authorize(Roles = "Admin, StoreOwner")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Category == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categorys/Create
        [Authorize(Roles = "Admin, StoreOwner")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categorys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, StoreOwner")]
        public async Task<IActionResult> Create([Bind("Id,Name,Status")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categorys/Edit/5
        [Authorize(Roles = "Admin, StoreOwner")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Category == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categorys/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, StoreOwner")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Status")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        // GET: Categorys/Delete/5
        [Authorize(Roles = "Admin, StoreOwner")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Category == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categorys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, StoreOwner")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Category == null)
            {
                return Problem("Entity set 'MvcBookContext.Category'  is null.");
            }
            var category = await _context.Category.FindAsync(id);
            if (category != null)
            {
                _context.Category.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return (_context.Category?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [Authorize(Roles = "Admin, StoreOwner")]
        public IActionResult ExportCategoryList()
        {
            //get data from database using EF
            List<Category> categories = _context.Category.ToList<Category>();
            var stream = new MemoryStream();
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("Categories");

                worksheet.Cells["A1"].Value = "Category List of FPT Book System";
                worksheet.Cells["A3"].Value = "ID";
                worksheet.Cells["B3"].Value = "Name";

                int row = 4;
                foreach (var category in categories)
                {
                    worksheet.Cells[row, 1].Value = category.Id;
                    worksheet.Cells[row, 2].Value = category.Name;
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
