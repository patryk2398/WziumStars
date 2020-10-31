using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WziumStars.Models.ViewModels;

namespace WziumStars.Models.ViewModels
{
    public class OrderListViewModel
    {
        public IList<OrderDetailsViewModel> Orders { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
