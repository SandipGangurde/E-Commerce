using DataCarrier.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCarrier.ApplicationModels.OrderDetails.Request
{
    public class PlaceOrder
    {
        public long UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<ProductItems> ProductItems { get; set; }
        public AddressesVM Address { get; set; }
        public List<long> CartItemIds { get; set; }
    }

    public class ProductItems
    {
        public long ProductId { get; set;}
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountApplied { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
