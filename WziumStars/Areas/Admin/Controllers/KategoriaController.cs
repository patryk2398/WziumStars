using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WziumStars.Data;

namespace WziumStars.Areas.Admin.Controllers
{
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


    }
}
