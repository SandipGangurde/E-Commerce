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
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public int StockQuantity { get; set; }
        public long? CategoryId { get; set; }
        public long? DiscountId { get; set; }
        public bool IsActive { get; set; }
        public string? ImageFileName { get; set; }
        public string? ImageFilePath { get; set; }
    }

}
