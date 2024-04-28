using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCarrier.ApplicationModels.Products.Response
{
    public class ProductDetailVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public bool ProductIsActive { get; set; }
        public bool CategoryIsActive { get; set; }
        public double AverageRating { get; set; }
        public int NumberOfReviews { get; set; }
        public long DiscountId { get; set; }
        public string DiscountCode { get; set; }
        public string DiscountDescription { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal DiscountPrice { get; set; }
        public decimal DiscountAmount { get; set; }

        public string? ImageFileName { get; set; }
        public string? ImageFilePath { get; set; }
    }
}
