using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WziumStars.Data;
using WziumStars.Models;
using WziumStars.Models.ViewModels;

namespace WziumStars.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PodkategoriaController : Controller
    {
        public readonly ApplicationDbContext _db;

        [TempData]
        public string StatusMessage { get; set; }

        public PodkategoriaController(ApplicationDbContext db)
        {
            _db = db;
        }

        //GET - INDEX
        public async Task<IActionResult> Index()
        {
            var podkategorie = await _db.Podkategoria.Include(s=>s.Kategoria).ToListAsync();
            return View(podkategorie);
        }

        //GET - CREATE
        public async Task<IActionResult> Dodaj()
        {
            PodkategoriaAndKategoriaViewModel model = new PodkategoriaAndKategoriaViewModel()
            {
                KategoriaList = await _db.Kategoria.ToListAsync(),
                Podkategoria = new Models.Podkategoria(),
                PodkategoriaList = await _db.Podkategoria.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync()
            };
            return View(model);
        }

        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dodaj(PodkategoriaAndKategoriaViewModel model)
        {
            if(ModelState.IsValid)
            {
                var doesPodkategoriaExist = _db.Podkategoria.Include(s => s.Kategoria).Where(s => s.Name == model.Podkategoria.Name && s.Kategoria.Id == model.Podkategoria.CategoryId);
                if(doesPodkategoriaExist.Count()>0)
                {
                    StatusMessage = "Error: Podkategoria " + doesPodkategoriaExist.First().Name + " istenieje już w kategorii " + doesPodkategoriaExist.First().Kategoria.Name;

                }
                else
                {
                    _db.Podkategoria.Add(model.Podkategoria);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            PodkategoriaAndKategoriaViewModel modelVM = new PodkategoriaAndKategoriaViewModel()
            {
                KategoriaList = await _db.Kategoria.ToListAsync(),
                Podkategoria = model.Podkategoria,
                PodkategoriaList = await _db.Podkategoria.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync(),
                StatusMessage = StatusMessage
            };
            return View(modelVM);
        }

        [ActionName("GetSubCategory")]
        public async Task<IActionResult> GetSubCategory(int id)
        {
            List<Podkategoria> podkategorie = new List<Podkategoria>();

            podkategorie = await (from podkategoria in _db.Podkategoria
                            where podkategoria.CategoryId == id
                            select podkategoria).ToListAsync();
            return Json(new SelectList(podkategorie, "Id", "Name"));
        }

        //GET - EDIT
        public async Task<IActionResult> Edytuj(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }

            var podkategoria = await _db.Podkategoria.SingleOrDefaultAsync(m => m.Id == id);

            if(podkategoria==null)
            {
                return NotFound();
            }

            PodkategoriaAndKategoriaViewModel model = new PodkategoriaAndKategoriaViewModel()
            {
                KategoriaList = await _db.Kategoria.ToListAsync(),
                Podkategoria = podkategoria,
                PodkategoriaList = await _db.Podkategoria.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync()
            };
            return View(model);
        }

        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edytuj(int id, PodkategoriaAndKategoriaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doesPodkategoriaExist = _db.Podkategoria.Include(s => s.Kategoria).Where(s => s.Name == model.Podkategoria.Name && s.Kategoria.Id == model.Podkategoria.CategoryId);
                if (doesPodkategoriaExist.Count() > 0)
                {
                    StatusMessage = "Error: Podkategoria " + doesPodkategoriaExist.First().Name + " istenieje już w kategorii " + doesPodkategoriaExist.First().Kategoria.Name;
                }
                else
                {
                    var podkategoriaFromDb = await _db.Podkategoria.FindAsync(id);
                    podkategoriaFromDb.Name = model.Podkategoria.Name;
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            PodkategoriaAndKategoriaViewModel modelVM = new PodkategoriaAndKategoriaViewModel()
            {
                KategoriaList = await _db.Kategoria.ToListAsync(),
                Podkategoria = model.Podkategoria,
                PodkategoriaList = await _db.Podkategoria.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync(),
                StatusMessage = StatusMessage
            };
            modelVM.Podkategoria.Id = id;
            return View(modelVM);
        }
    }
}
