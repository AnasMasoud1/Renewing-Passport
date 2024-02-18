using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RenewingPassport.Data;
using RenewingPassport.Models;

namespace RenewingPassport.Controllers
{
    public class PassportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PassportsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;   
        }

        // GET: Passports
        public async Task<IActionResult> Index()
        {
              return _context.passports != null ? 
                          View(await _context.passports.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.passports'  is null.");
        }

        // GET: Passports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.passports == null)
            {
                return NotFound();
            }

            var passport = await _context.passports
                .FirstOrDefaultAsync(m => m.Id == id);
            if (passport == null)
            {
                return NotFound();
            }

            return View(passport);
        }

        // GET: Passports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Passports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,UserID,Email,Date,PassportImage,IDImage,Status,PassportFormFile,IDImageFile")] Passport passport)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (passport.PassportFormFile != null && passport.IDImageFile != null)
        //        {
        //            string wwwRootPath = _webHostEnvironment.WebRootPath;
        //            string fileName = Guid.NewGuid().ToString() + "_" + passport.PassportFormFile.FileName;
        //            //string extention = Path.GetExtension(passport.ImageFile.FileName).ToLower();
        //            string path = Path.Combine(wwwRootPath + "/Imgs/" + fileName);

        //            using (var fileStream = new FileStream(path, FileMode.Create))
        //            {
        //                await passport.PassportFormFile.CopyToAsync(fileStream);
        //            }
        //            passport.PassportImage = fileName;
        //            //passport.UserID = User.Identity.Name;
        //            passport.Status = "Prossing";
        //        }
        //        _context.Add(passport);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(passport);
        //}
        public async Task<IActionResult> Create([Bind("Id,UserID,Email,Date,PassportImage,IDImage,Status,PassportFormFile,IDImageFile")] Passport passport)
        {
            if (ModelState.IsValid)
            {
                if (passport.PassportFormFile != null && passport.IDImageFile != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;

                    // Process Passport Form File
                    string passportFileName = Guid.NewGuid().ToString() + "_" + passport.PassportFormFile.FileName;
                    string passportFilePath = Path.Combine(wwwRootPath, "Imgs", passportFileName);

                    using (var passportFileStream = new FileStream(passportFilePath, FileMode.Create))
                    {
                        await passport.PassportFormFile.CopyToAsync(passportFileStream);
                    }
                    passport.PassportImage = passportFileName;

                    // Process ID Image File
                    string idFileName = Guid.NewGuid().ToString() + "_" + passport.IDImageFile.FileName;
                    string idFilePath = Path.Combine(wwwRootPath, "Imgs", idFileName);

                    using (var idFileStream = new FileStream(idFilePath, FileMode.Create))
                    {
                        await passport.IDImageFile.CopyToAsync(idFileStream);
                    }
                    passport.IDImage = idFileName;

                    passport.Status = "Processing";
                }

                _context.Add(passport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(passport);
        }

        // GET: Passports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.passports == null)
            {
                return NotFound();
            }

            var passport = await _context.passports.FindAsync(id);
            if (passport == null)
            {
                return NotFound();
            }
            return View(passport);
        }

        // POST: Passports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserID,Email,Date,PassportImage,IDImage,Status")] Passport passport)
        {
            if (id != passport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(passport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PassportExists(passport.Id))
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
            return View(passport);
        }

        // GET: Passports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.passports == null)
            {
                return NotFound();
            }

            var passport = await _context.passports
                .FirstOrDefaultAsync(m => m.Id == id);
            if (passport == null)
            {
                return NotFound();
            }

            return View(passport);
        }

        // POST: Passports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.passports == null)
            {
                return Problem("Entity set 'ApplicationDbContext.passports'  is null.");
            }
            var passport = await _context.passports.FindAsync(id);
            if (passport != null)
            {
                _context.passports.Remove(passport);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PassportExists(int id)
        {
          return (_context.passports?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
