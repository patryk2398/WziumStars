using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WziumStars.Data;
using WziumStars.Models;
using WziumStars.Utility;

namespace WziumStars.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.AdminEndUser)]
    [Area("Admin")]
    public class KategoriaController : Controller
    {
        public readonly ApplicationDbContext _db;

        public KategoriaController(ApplicationDbContext db)
        {
            _db = db;
        }

        //GET
        public async Task<IActionResult> Index()
        {
            return View(await _db.Kategoria.ToListAsync());
        }

        //GET
        public IActionResult Dodaj()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dodaj(Kategoria kategoria)
        {
            if(ModelState.IsValid)
            {
                _db.Kategoria.Add(kategoria);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(kategoria);
        }

        //GET
        public async Task<IActionResult> Edytuj(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var kategoria = await _db.Kategoria.FindAsync(id);
            if(kategoria==null)
            {
                return NotFound();
            }
            return View(kategoria);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edytuj(Kategoria kategoria)
        {
            if (ModelState.IsValid)
            {
                _db.Update(kategoria);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(kategoria);
        }

        //GET
        public async Task<IActionResult> Usun(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var kategoria = await _db.Kategoria.FindAsync(id);
            if (kategoria == null)
            {
                return NotFound();
            }
            return View(kategoria);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Usun(int id)
        {
            var kategoria = await _db.Kategoria.FindAsync(id);
            if(kategoria==null)
            {
                return View();
            }

            _db.Kategoria.Remove(kategoria);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
