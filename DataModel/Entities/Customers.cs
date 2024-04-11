using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Entities
{
    [Table("Customers")]
    public class Customers
    {
        [Key]
        public long CustomerId { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
    }
}
