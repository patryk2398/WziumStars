using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WziumStars.Data;
using WziumStars.Models.ViewModels;
using WziumStars.Utility;

namespace WziumStars.Areas.Klient.Controllers
{
    [Area("Klient")]
    public class ProduktController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public ProduktViewModel ProduktVM { get; set; }

        public ProduktController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            ProduktVM = new ProduktViewModel()
            {
                Kategoria = _db.Kategoria,
                Produkt = new Models.Produkt()
            };
        }

        public async Task<IActionResult> Index()
        {
            var Produkty = await _db.Produkt.ToListAsync();
            return View(Produkty);
        }

        public IActionResult Dodaj()
        {
            return View(ProduktVM);
        }

        //CREATE - POST
        [HttpPost, ActionName("Dodaj")]
        public async Task<IActionResult> DodajPOST()
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            _db.Produkt.Add(ProduktVM.Produkt);
            await _db.SaveChangesAsync();

            string webRootPath = _webHostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var produktFromDb = await _db.Produkt.FindAsync(ProduktVM.Produkt.Id);

            if (files.Count > 0)
            {
                string[] imagesTable = new string[files.Count];
                for(int i = 0; i < files.Count; i++)
                {
                    var uploads = Path.Combine(webRootPath, "img");
                    var extension = Path.GetExtension(files[i].FileName);

                    using (var fileStream = new FileStream(Path.Combine(uploads, ProduktVM.Produkt.Id + "."+ i + extension), FileMode.Create))
                    {
                        files[i].CopyTo(fileStream);
                    }
                    imagesTable[i] = @"\img\" + ProduktVM.Produkt.Id + "." + i + extension;
                }
                StringBuilder allImages = new StringBuilder();
                for(int i = 0; i < files.Count; i++)
                {
                    allImages.Append(imagesTable[i]);
                    allImages.Append("|");
                }
                produktFromDb.Images = allImages.ToString();
                produktFromDb.Avatar = imagesTable[0];
            }
            else
            {
                var uploads = Path.Combine(webRootPath, @"images\" + SD.DefaultProduktImage);
                System.IO.File.Copy(uploads, webRootPath + @"\img\" + ProduktVM.Produkt.Id + ".png");
                produktFromDb.Images = @"\img\" + ProduktVM.Produkt.Id + ".png";
                produktFromDb.Avatar = @"\img\" + ProduktVM.Produkt.Id + ".png";
            }
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
