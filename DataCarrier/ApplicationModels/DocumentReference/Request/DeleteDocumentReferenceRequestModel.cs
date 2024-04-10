using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCarrier.ApplicationModels.DocumentReference.Request
{
    public class DeleteDocumentReferenceRequestModel
    {
        public string TableName { get; set; }
        public string PrimaryKeyValue { get; set; }
        public long DocumentID { get; set; }
    }
}
