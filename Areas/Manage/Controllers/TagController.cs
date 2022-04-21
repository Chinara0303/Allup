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
    [Area("Manage")]
    public class TagController : Controller
    {
        private readonly AppDbContext _context;
        public TagController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tags.OrderByDescending(t => t.CreatedAt).ToListAsync());
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tag tag)
        {
            if (!ModelState.IsValid) return View();


            if (string.IsNullOrWhiteSpace(tag.Name))
            {
                ModelState.AddModelError("Name", "Bosluq Olmamalidir");
                return View();
            }

            if (tag.Name.CheckString())
            {
                ModelState.AddModelError("Name", "Yalniz herf ola biler");
                return View();
            }


            if (await _context.Tags.AnyAsync(t => t.Name.ToLower() == tag.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Artiq movcuddur");
                return View();
            }
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            Tag tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);
            if (tag == null) return NotFound();

            return View(tag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Tag tag)
        {
            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();

            if (id != tag.Id) return BadRequest();

            Tag dbtag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);

            if (dbtag == null) return NotFound();

            if (string.IsNullOrWhiteSpace(tag.Name))
            {
                ModelState.AddModelError("Name", "Bosluq Olmamalidir");
                return View(tag);
            }

            if (tag.Name.CheckString())
            {
                ModelState.AddModelError("Name", "Yalniz Herf Ola Biler");
                return View(tag);
            }

            if (await _context.Tags.AnyAsync(t => t.Id != tag.Id && t.Name.ToLower() == tag.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Artiq Movcuddur");
                return View(tag);
            }


            dbtag.Name = tag.Name;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
       
        public async Task<IActionResult> Delete(int? id)
        {
            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();

            Tag dbtag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);

            if (dbtag == null) return NotFound();

            dbtag.IsDeleted = true;
            dbtag.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return PartialView("_TagIndexPartial", await _context.Tags.OrderByDescending(t => t.CreatedAt).ToListAsync());
        }
        public async Task<IActionResult> Restore(int? id)
        {
            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();

            Tag dbtag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);

            if (dbtag == null) return NotFound();

            dbtag.IsDeleted = false;

            await _context.SaveChangesAsync();

            return PartialView("_TagIndexPartial", await _context.Tags.OrderByDescending(t => t.CreatedAt).ToListAsync());
        }
    }
}
