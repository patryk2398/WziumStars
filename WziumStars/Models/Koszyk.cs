using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WziumStars.Models
{
    public class Koszyk
    {
        public Koszyk()
        {
            Count = 1;
        }

        [Key]
        public int Id { get; set; }

        public string ApplicationUserId { get; set; }
        [NotMapped]
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public int ProduktId { get; set; }
        [NotMapped]
        [ForeignKey("ProduktId")]
        public virtual Produkt Produkt { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Wprowadź ilość większą lub równą 1")]
        public int Count { get; set; }

        public System.DateTime DateCreated { get; set; }
    }
}
