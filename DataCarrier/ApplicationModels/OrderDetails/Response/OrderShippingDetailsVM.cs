using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCarrier.ApplicationModels.OrderDetails.Response
{
    public class OrderShippingDetailsVM
    {
        public long OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsCompleted { get; set; }
        public string ShippingMethod { get; set; }
        public string ShippingAddress { get; set; }
        public string TrackingNumber { get; set; }
        public DateTime ShippedDate { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}
