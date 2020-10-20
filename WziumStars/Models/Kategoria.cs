using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WziumStars.Models
{
    public class Kategoria
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
    }
}
