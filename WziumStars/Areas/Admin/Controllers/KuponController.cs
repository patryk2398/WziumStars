using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WziumStars.Data;
using WziumStars.Models;

namespace WziumStars.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class KuponController : Controller
    {
        private readonly ApplicationDbContext _db;
        public KuponController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _db.Kupon.ToListAsync());
        }

        [BindProperty]
        public Kupon kupon { get; set; }

        public IActionResult Dodaj()
        {
            return View();
        }

        [HttpPost, ActionName("Dodaj")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DodajPOST()
        {
            if (ModelState.IsValid)
            {
                _db.Kupon.Add(kupon);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kupon);

        }

        //EDIT - GET
        public async Task<IActionResult> Edytuj(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var kupon = await _db.Kupon.FindAsync(id);
            if (kupon == null)
            {
                return NotFound();
            }

            return View(kupon);
        }

        //EDIT - POST
        [HttpPost, ActionName("Edytuj")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EdytujPOST()
        {
            if (!ModelState.IsValid)
            {
                return View(kupon);
            }
            _db.Update(kupon);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //GET - DELETE
        public async Task<IActionResult> Usun(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var kupon = await _db.Kupon.FindAsync(id);
            if (kupon == null)
            {
                return NotFound();
            }
            return View(kupon);
        }

        [HttpPost, ActionName("Usun")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UsunPOST(int? id)
        {
            var kupon = await _db.Kupon.FindAsync(id);

            if (kupon == null)
            {
                return View();
            }
            _db.Kupon.Remove(kupon);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
