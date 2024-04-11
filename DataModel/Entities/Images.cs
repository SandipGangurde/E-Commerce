using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Entities
{
    [Table("Images")]

    public class Images
    {
        [Key]
        public long ImageId { get; set; }
        public string FileName { get; set; }
        public string TableName { get; set; }
        public long RecordId { get; set; }
        public string    FilePath { get; set; }
    }
}
