using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCarrier.ViewModels
{
    public class WishlistVM
    {
        public long WishlistId { get; set; }
        public long UserId { get; set; }
        public long ProductId { get; set; }
    }
}
