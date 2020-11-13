using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WziumStars.Models
{
    public class ApplicationUser : IdentityUser
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

        [Display(Name = "Nr mieszkania")]
        public string ApartmentNumber { get; set; }

        [Display(Name = "Kod pocztowy")]
        public string PostalCode { get; set; }
    }
}
