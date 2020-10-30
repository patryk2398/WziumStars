using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WziumStars.Models.ViewModels
{
    public class OrderDetailsKoszyk
    {
        public List<Koszyk> listCart { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}
