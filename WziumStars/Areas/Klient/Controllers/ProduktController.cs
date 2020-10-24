using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WziumStars.Data;
using WziumStars.Models;
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
            ProduktVM.Produkt.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());
            int test = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());
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
                for (int i = 0; i < files.Count; i++)
                {
                    var uploads = Path.Combine(webRootPath, "imgProdukt");
                    var extension = Path.GetExtension(files[i].FileName);

                    using (var fileStream = new FileStream(Path.Combine(uploads, ProduktVM.Produkt.Id + "." + i + extension), FileMode.Create))
                    {
                        files[i].CopyTo(fileStream);
                    }
                    imagesTable[i] = @"\imgProdukt\" + ProduktVM.Produkt.Id + "." + i + extension;
                }
                StringBuilder allImages = new StringBuilder();
                for (int i = 0; i < files.Count; i++)
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
                System.IO.File.Copy(uploads, webRootPath + @"\imgProdukt\" + ProduktVM.Produkt.Id + ".png");
                produktFromDb.Images = @"\imgProdukt\" + ProduktVM.Produkt.Id + ".png";
                produktFromDb.Avatar = @"\imgProdukt\" + ProduktVM.Produkt.Id + ".png";
            }
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //EDIT - GET
        public async Task<IActionResult> Edytuj(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProduktVM.Produkt = await _db.Produkt.Include(m => m.Category).Include(m => m.SubCategory).SingleOrDefaultAsync(m => m.Id == id);
            ProduktVM.Podkategoria = await _db.Podkategoria.Where(s => s.CategoryId == ProduktVM.Produkt.CategoryId).ToListAsync();
            
            if(ProduktVM.Produkt == null)
            {
                return NotFound();
            }

            return View(ProduktVM);
        }

        //EDIT - POST
        [HttpPost, ActionName("Edytuj")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EdytujPOST(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProduktVM.Produkt.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());

            if (!ModelState.IsValid)
            {
                ProduktVM.Podkategoria = await _db.Podkategoria.Where(s => s.CategoryId == ProduktVM.Produkt.CategoryId).ToListAsync();
                return View(ProduktVM);
            }

            string webRootPath = _webHostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            var ProduktFromDb = await _db.Produkt.FindAsync(ProduktVM.Produkt.Id);

            if (files.Count > 0)
            {
                var uploads = Path.Combine(webRootPath, "imgProdukt");
                int count = ProduktFromDb.Images.Count(f => f == '|');
                for (int i = 0; i < count; i++)
                {
                    string[] array = ProduktFromDb.Images.Split('|');
                    var imagePath = Path.Combine(webRootPath, array[i].TrimStart('\\'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                string[] imagesTable = new string[files.Count];
                for (int i = 0; i < files.Count; i++)
                {
                    var extension = Path.GetExtension(files[i].FileName);

                    using (var fileStream = new FileStream(Path.Combine(uploads, ProduktVM.Produkt.Id + "." + i + extension), FileMode.Create))
                    {
                        await files[i].CopyToAsync(fileStream);
                    }
                    imagesTable[i] = @"\imgProdukt\" + ProduktVM.Produkt.Id + "." + i + extension;
                }
                StringBuilder allImages = new StringBuilder();
                for (int i = 0; i < files.Count; i++)
                {
                    allImages.Append(imagesTable[i]);
                    allImages.Append("|");
                }
                ProduktFromDb.Images = allImages.ToString();
                ProduktFromDb.Avatar = imagesTable[0];
            }

            ProduktFromDb.Name = ProduktVM.Produkt.Name;
            ProduktFromDb.Description = ProduktVM.Produkt.Description;
            ProduktFromDb.Price = ProduktVM.Produkt.Price;
            ProduktFromDb.CategoryId = ProduktVM.Produkt.CategoryId;
            ProduktFromDb.SubCategoryId = ProduktVM.Produkt.SubCategoryId;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //GET - DETAILS
        public async Task<IActionResult> Szczegoly(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProduktVM.Produkt = await _db.Produkt.Include(m => m.Category).Include(m => m.SubCategory).SingleOrDefaultAsync(m => m.Id == id);
            ProduktVM.Podkategoria = await _db.Podkategoria.Where(s => s.CategoryId == ProduktVM.Produkt.CategoryId).ToListAsync();

            if (ProduktVM.Produkt == null)
            {
                return NotFound();
            }

            return View(ProduktVM);
        }

        //GET - DELETE
        public async Task<IActionResult> Usun(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProduktVM.Produkt = await _db.Produkt.Include(m => m.Category).Include(m => m.SubCategory).SingleOrDefaultAsync(m => m.Id == id);
            ProduktVM.Podkategoria = await _db.Podkategoria.Where(s => s.CategoryId == ProduktVM.Produkt.CategoryId).ToListAsync();

            if (ProduktVM.Produkt == null)
            {
                return NotFound();
            }

            return View(ProduktVM);
        }

        //POST - DELETE
        [HttpPost, ActionName("Usun")]
        public async Task<IActionResult> UsunPOST(int? id)
        {
            var produkt = await _db.Produkt.FindAsync(id);

            if (produkt == null)
            {
                return View();
            }

            string webRootPath = _webHostEnvironment.WebRootPath;
            var uploads = Path.Combine(webRootPath, "imgProdukt");
            int count = produkt.Images.Count(f => f == '|');
            for (int i = 0; i < count; i++)
            {
                string[] array = produkt.Images.Split('|');
                var imagePath = Path.Combine(webRootPath, array[i].TrimStart('\\'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _db.Produkt.Remove(produkt);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
