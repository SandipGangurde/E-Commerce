using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Entities
{
    [Table("Wishlist")]
    public class Wishlist
    {
        [Key]
        public long WishlistId { get; set; }
        public long UserId { get; set; }
        public long ProductId { get; set; }
    }
}
