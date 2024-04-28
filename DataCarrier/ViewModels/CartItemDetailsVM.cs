using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCarrier.ViewModels
{
    public class CartItemDetailsVM
    {
        public int CartItemId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public decimal OriginalPrice { get; set; }
        public int StockQuantity { get; set; }
        public int CartQuantity { get; set; }
        public decimal DiscountedPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public decimal TotalDiscountedPrice { get; set; }
        public string DiscountType { get; set; }
        public decimal? DiscountAmount { get; set; }
        public bool CategoryStatus { get; set; }
        public bool ProductAvailability { get; set; }
        public bool ProductStatus { get; set; }
        public string? ImageFileName { get; set; }
        public string? ImageFilePath { get; set; }
    }
}
