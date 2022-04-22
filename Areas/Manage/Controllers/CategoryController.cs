using Allup.DAL;
using Allup.Extensions;
using Allup.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Areas.Manage.Controllers
{
    [Area("manage")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(bool? status, int page = 1)
        {
            ViewBag.Status = status;
            IEnumerable<Category> categories = await _context.Categories
                .Include(b => b.Products)
                .Where(b => status != null ? b.IsDeleted == status : true)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)categories.Count() / 5);

            return View(categories.Skip((page - 1) * 5).Take(5));
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.MainCategory = await _context.Categories.Where(c => c.IsMain && !c.IsDeleted).ToListAsync();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid) return View();

            if (string.IsNullOrWhiteSpace(category.Name))
            {
                ModelState.AddModelError("Name", "Bow ola bilmez");
                return View();
            }
            if (category.Name.CheckString())
            {
                ModelState.AddModelError("Name", "Yalniz herf ola biler");
                return View();
            }
            if (await _context.Categories.AnyAsync(b => b.Name.ToLower() == category.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Artiq Movcuddur");
                return View();
            }

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
