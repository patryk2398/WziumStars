using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WziumStars.Models
{
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }


        [Required]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Numer telefonu")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Display(Name = "Kraj")]
        public string Country { get; set; }

        [Display(Name = "Stan")]
        public string State { get; set; }

        [Required]
        [Display(Name = "Miejscowość")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Ulica")]
        public string Street { get; set; }

        [Required]
        [Display(Name = "Nr domu")]
        public string HouseNumber { get; set; }

        [Display(Name = "Nr mieszkania")]
        public string ApartmentNumber { get; set; }

        [Required]
        [Display(Name = "Kod pocztowy")]
        public string PostalCode { get; set; }

        [Display(Name = "firma")]
        public string Firm { get; set; }



        [Required]
        [Display(Name = "Data zamówienia")]
        public DateTime OrderDate { get; set; }

        [Required]
        public double OrderTotalOriginal { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Cena całkowita")]
        public double OrderTotal { get; set; }

        [Display(Name = "Kupon")]
        public string CouponCode { get; set; }

        public double CouponCodeDiscount { get; set; }
        public string Status { get; set; }
        public string PaymentStatus { get; set; }

        [Display(Name = "Uwagi")]
        public string Comments { get; set; }  

        public string PayUId { get; set; }

        [Display(Name = "Dostawa")]
        public string Delivery { get; set; }

        [Display(Name = "Dostawa")]
        public double DeliveryCost { get; set; }

        [Display(Name = "Metoda płatności")]
        public string PayMethod { get; set; }
    }
}
