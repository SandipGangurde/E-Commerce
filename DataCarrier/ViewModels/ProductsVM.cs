using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCarrier.ViewModels
{
    public class ProductsVM
    {
        public long ProductId { get; set; }
        public string ProductsName { get; set; }
        public string ProductsDescription { get; set; }
        public decimal ProductsPrice { get; set; }
        public int StockQuantity { get; set; }
        public long? CategoryId { get; set; }
        public bool IsActive { get; set; }
    }

}
