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
    public class BrandController : Controller
    {
        private readonly AppDbContext _context;
        public BrandController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(bool? status, int page = 1)
        {
            ViewBag.Status = status;
            IEnumerable<Brand> brands = await _context.Brands
                .Include(b => b.Products)
                .Where(b => status != null ? b.IsDeleted == status : true)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)brands.Count() / 5);

            return View(brands.Skip((page - 1) * 5).Take(5));

            
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand brand)
        {
            if (!ModelState.IsValid) return View();

            if(string.IsNullOrWhiteSpace(brand.Name))
            {
                ModelState.AddModelError("Name", "Bow ola bilmez");
                return View();
            }
            if (brand.Name.CheckString())
            {
                ModelState.AddModelError("Name", "Yalniz herf ola biler");
                return View();
            }
            if(await _context.Brands.AnyAsync(b => b.Name.ToLower() == brand.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Artiq Movcuddur");
                return View();
            }

            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int? id,bool? status,int page=1)
        {
            if (id == null) return BadRequest();

            Brand brand = await _context.Brands.FirstOrDefaultAsync(b => b.Id==id);

            if (brand == null) return NotFound();

            return View(brand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Brand brand,bool? status,int page=1)
        {
            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();

            Brand dbbrand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);

            if (dbbrand == null) return NotFound();

            if (string.IsNullOrWhiteSpace(brand.Name))
            {
                ModelState.AddModelError("Name", "Bow ola bilmez");
                return View(brand);
            }
            if (brand.Name.CheckString())
            {
                ModelState.AddModelError("Name", "Yalniz herf ola biler");
                return View(brand);
            }
            if (await _context.Brands.AnyAsync(b => b.Id==id && b.Name.ToLower() == brand.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Artiq Movcuddur");
                return View(brand);
            }

            dbbrand.Name = brand.Name;
            await _context.SaveChangesAsync();

            ViewBag.Status = status;

            IEnumerable<Brand> brands = await _context.Brands
                .Include(b => b.Products)
                .Where(b => status != null ? b.IsDeleted == status : true)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)brands.Count()/5);
            
            return RedirectToAction("Index",new { status=status,page=page});
        }

        public async Task<IActionResult> Delete(int? id,bool? status, int page=1)
        {
            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();

            Brand dbbrand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);

            if (dbbrand == null) return NotFound();

            dbbrand.IsDeleted = true;
            dbbrand.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();


            ViewBag.Status = status;

            IEnumerable<Brand> brands = await _context.Brands
                .Include(b => b.Products)
                .Where(b => status != null ? b.IsDeleted == status : true)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)brands.Count() / 5);

            return PartialView("_BrandIndexPartial",brands.Skip((page-1)*5).Take(5));
        }
        public async Task<IActionResult> Restore(int? id,bool? status, int page=1)
        {
            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();

            Brand dbbrand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);

            if (dbbrand == null) return NotFound();

            dbbrand.IsDeleted = false;

            await _context.SaveChangesAsync();

            ViewBag.Status = status;

            IEnumerable<Brand> brands = await _context.Brands
                .Include(b => b.Products)
                .Where(b => status != null ? b.IsDeleted == status : true)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)brands.Count() / 5);

            return PartialView("_BrandIndexPartial", brands.Skip((page - 1) * 5).Take(5));

          
        }
    }
}
