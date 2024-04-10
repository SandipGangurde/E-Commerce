using Dapper.Contrib.Extensions;

namespace DataModel.Entities
{
    [Table("Documents")]
    public class Documents : BaseEntity
    {
        [Key]
        public long DocumentID { get; set; }

        public string FileName { get; set; }
        public string FileExt { get; set; }
        public decimal FileSize { get; set; }
        public string FilePath { get; set; }
        public byte[] FileData { get; set; }
        public string DocumentURL { get; set; }
        public string FileRemark { get; set; }
    }
}
