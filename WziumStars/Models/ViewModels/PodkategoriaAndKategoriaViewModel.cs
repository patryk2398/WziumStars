using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WziumStars.Models.ViewModels
{
    public class PodkategoriaAndKategoriaViewModel
    {
        public IEnumerable<Kategoria> KategoriaList { get; set; }
        public Podkategoria Podkategoria { get; set; }
        public IEnumerable<string> PodkategoriaList { get; set; }
        public string StatusMessage { get; set; }
    }
}
