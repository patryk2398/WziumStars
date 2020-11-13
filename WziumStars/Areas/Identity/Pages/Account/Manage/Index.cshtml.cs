using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WziumStars.Data;
using WziumStars.Models;

namespace WziumStars.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _db;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Imię")]
            public string FirstName { get; set; }

            [Display(Name = "Nazwisko")]
            public string LastName { get; set; }

            [Display(Name = "Kraj")]
            public string Country { get; set; }

            [Display(Name = "Stan")]
            public string State { get; set; }

            [Display(Name = "Miejscowość")]
            public string City { get; set; }

            [Display(Name = "Ulica i numer budynku")]
            public string Street { get; set; }

            [Display(Name = "Numer mieszkania")]
            public string ApartmentNumber { get; set; }
            
            [Display(Name = "Kod pocztowy")]
            public string PostalCode { get; set; }

            [Phone(ErrorMessage = "Błędna składnia numeru telefonu")]
            [Display(Name = "Telefon")]
            public string PhoneNumber { get; set; }

            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
        }

        private void Load(ApplicationUser user)
        {
            Input = new InputModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Country = user.Country,
                State = user.State,
                City = user.City,
                Street = user.Street,
                ApartmentNumber = user.ApartmentNumber,
                PostalCode = user.PostalCode,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ApplicationUser applicationUser = await _db.ApplicationUser.Where(c => c.Id == claim.Value).FirstOrDefaultAsync();
            if (applicationUser == null)
            {
                return NotFound();
            }

            Load(applicationUser);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                ApplicationUser applicationUser = await _db.ApplicationUser.Where(c => c.Id == claim.Value).FirstOrDefaultAsync();
                applicationUser.FirstName = Input.FirstName;
                applicationUser.LastName = Input.LastName;
                applicationUser.Country = Input.Country;
                applicationUser.State = Input.State;
                applicationUser.City = Input.City;
                applicationUser.Street = Input.Street;
                applicationUser.ApartmentNumber = Input.ApartmentNumber;
                applicationUser.PostalCode = Input.PostalCode;
                applicationUser.PhoneNumber = Input.PhoneNumber;
                
                await _db.SaveChangesAsync();
                StatusMessage = "Pomyślnie zmieniono dane użytkownika.";
                return RedirectToPage();
            }
            StatusMessage = "Error: Wystąpił błąd podczas zapisywania danych profilu.";
            return RedirectToPage();
        }
    }
}
