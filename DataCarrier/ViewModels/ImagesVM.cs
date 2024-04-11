using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCarrier.ViewModels
{
    public class ImagesVM
    {
        public long ImageId { get; set; }
        public string FileName { get; set; }
        public string TableName { get; set; }
        public long RecordId { get; set; }
        public string FilePath { get; set; }
    }
}
