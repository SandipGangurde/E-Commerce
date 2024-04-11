using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Entities
{
    [Table("Reviews")]
    public class Reviews
    {
        [Key]
        public long ReviewId { get; set; }
        public long ProductId { get; set; }
        public long UserId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}
