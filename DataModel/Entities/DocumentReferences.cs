using Dapper.Contrib.Extensions;
using System;

namespace DataModel.Entities
{
    [Table("DocumentReferences")]
    public class DocumentReferences : BaseEntity
    {
        [Key]
        public long DocumentReferenceID { get; set; }
        public string TableName { get; set; }
        public string PrimaryKeyValue { get; set; }
        public long DocumentID { get; set; }
    }
}
