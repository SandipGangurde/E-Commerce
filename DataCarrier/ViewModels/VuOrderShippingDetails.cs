using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCarrier.ViewModels
{
    public class VuOrderShippingDetails
    {
        public long OrderId { get; set; }
        public long UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsCompleted { get; set; }
        public long ShippingId { get; set; }
        public string ShippingMethod { get; set; }
        public string ShippingAddress { get; set; }
        public string TrackingNumber { get; set; }
        public DateTime ShippedDate { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}
