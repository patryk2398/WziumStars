using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WziumStars.Models
{
    public class Produkt
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name ="Nazwa")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Display(Name = "Rozmiar")]
        public string Size { get; set; }

        [Display(Name = "Kolor")]
        public string Color { get; set; }

        [Display(Name = "Rodzaj")]
        public string Type { get; set; }

        [Display(Name = "Zdjęcia")]
        public string Images { get; set; }

        [Display(Name = "Kategoria")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Kategoria Category { get; set; }

        [Display(Name = "Podkategoria")]
        public int SubCategoryId { get; set; }

        [ForeignKey("SubCategoryId")]
        public virtual Podkategoria SubCategory { get; set; }

        [Display(Name = "Cena")]
        public double Price { get; set; }
    }
}
