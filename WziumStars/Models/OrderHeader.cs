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
        [Display(Name = "Ulica i numer budynku")]
        public string Street { get; set; }

        [Display(Name = "Numer mieszkania")]
        public string ApartmentNumber { get; set; }

        [Required]
        [Display(Name = "Kod pocztowy")]
        public string PostalCode { get; set; }

        [Display(Name = "Firma")]
        public string Company { get; set; }


        [Display(Name = "Dostawa na inny adres")]
        public bool AnotherDeliveryAddress { get; set; }

        [Display(Name = "Imię")]
        public string DeliveryFirstName { get; set; }

        [Display(Name = "Nazwisko")]
        public string DeliveryLastName { get; set; }

        [Display(Name = "Kraj")]
        public string DeliveryCountry { get; set; }

        [Display(Name = "Stan")]
        public string DeliveryState { get; set; }

        [Display(Name = "Miejscowość")]
        public string DeliveryCity { get; set; }

        [Display(Name = "Ulica")]
        public string DeliveryStreet { get; set; }

        [Display(Name = "Numer mieszkania")]
        public string DeliveryApartmentNumber { get; set; }

        [Display(Name = "Kod pocztowy")]
        public string DeliveryPostalCode { get; set; }

        [Display(Name = "Firma")]
        public string DeliveryCompany { get; set; }



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
        public string DeliveryMethod { get; set; }

        [Display(Name = "Dostawa")]
        public double DeliveryCost { get; set; }

        public string Paczkomat { get; set; }

        [Display(Name = "Metoda płatności")]
        public string PayMethod { get; set; }
    }
}
