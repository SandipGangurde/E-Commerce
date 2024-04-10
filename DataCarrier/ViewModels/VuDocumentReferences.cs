using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCarrier.ViewModels
{
    public class VuDocumentReferences
    {
        public long DocumentReferenceID { get; set; }
        public string TableName { get; set; }
        public string PrimaryKeyValue { get; set; }
        public long DocumentID { get; set; }
        public string FileName { get; set; }
        public string FileExt { get; set; }
        public decimal FileSize { get; set; }
        public string FilePath { get; set; }
        public byte[] FileData { get; set; }
        public Guid DocumentGUID { get; set; }
        public string DocumentURL { get; set; }
    }
}
