using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Entities
{
    public class BaseEntity
    {
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
