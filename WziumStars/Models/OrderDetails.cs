using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WziumStars.Models
{
    public class OrderDetails
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public virtual OrderHeader OrderHeader { get; set; }

        [Required]
        public int ProduktId { get; set; }

        [ForeignKey("ProduktId")]
        public virtual Produkt Produkt { get; set; }

        [Display(Name = "Ilość")]
        public int Count { get; set; }
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Cena")]
        public double Price { get; set; }
    }
}
