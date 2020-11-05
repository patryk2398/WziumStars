using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WziumStars.Data;
using WziumStars.Models.ViewModels;

namespace WziumStars.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public MenuViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            PodkategoriaAndKategoriaViewModel podkategoriaAndKategoriaViewModel = new PodkategoriaAndKategoriaViewModel()
            {
                KategoriaList = await _db.Kategoria.ToListAsync(),
                PodkategoriaList = await _db.Podkategoria.ToListAsync()
            };


            return View(podkategoriaAndKategoriaViewModel);
        }
    }
}
