using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WziumStars.Models.ViewModels
{
    public class ProduktViewModel
    {
        public Produkt Produkt { get; set; }
        public IEnumerable<Kategoria> Kategoria { get; set; }
        public IEnumerable<Podkategoria> Podkategoria { get; set; }
    }
}
