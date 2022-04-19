using Allup.DAL;
using Allup.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Allup.Extensions;
using Microsoft.AspNetCore.Hosting;
using Allup.Helpres;

namespace Allup.Areas.Manage.Controllers
{
    [Area("manage")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SettingController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.settings.FirstOrDefaultAsync());
        }
        public async Task<IActionResult> Detail()
        {
            return View(await _context.settings.FirstOrDefaultAsync());
        }
        public async Task<IActionResult> Update()
        {
            return View(await _context.settings.FirstOrDefaultAsync());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Setting setting)
        {
            Setting dbSetting = await _context.settings.FirstOrDefaultAsync();
            setting.Logo = dbSetting.Logo;

            if (!ModelState.IsValid) return View(setting);

            if (setting.LogoImage != null)
            {
                if (!setting.LogoImage.CheckContentType("image/png"))
                {
                    ModelState.AddModelError("LogoImage", "Wekiller png formatda olmalidir");
                    return View();
                }
                if (!setting.LogoImage.CheckFileSize(30))
                {
                    ModelState.AddModelError("LogoImage", "Weklin olcusu 30kb dan cox olmamalidir");
                    return View();
                }
                Helper.DeleteFile(_env,dbSetting.Logo, "assets", "images");
                dbSetting.Logo = setting.LogoImage.CreateFile(_env, "assets", "images");
                
            }

            dbSetting.Offer = setting.Offer;
            dbSetting.Phone = setting.Phone;
            dbSetting.SupportPhone = setting.SupportPhone;
            dbSetting.WorkDay = setting.WorkDay;
            dbSetting.Email = setting.Email;
            dbSetting.UpdateAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
