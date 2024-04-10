using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCarrier.ViewModels
{
    public class DocumentReferencesVM
    {
        public long ModifiedBy { get; set; }
        public long DocumentReferenceID { get; set; }
        public string TableName { get; set; }
        public string PrimaryKeyValue { get; set; }
        public long DocumentID { get; set; }
    }
}
