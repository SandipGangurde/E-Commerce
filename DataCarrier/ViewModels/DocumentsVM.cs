using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataCarrier.ViewModels
{
    public class DocumentsVM
    {
        public long ModifiedBy { get; set; }
        public long DocumentID { get; set; }
        public string FileName { get; set; }
        public string FileExt { get; set; }
        public decimal FileSize { get; set; }
        public string FilePath { get; set; }
        public string FileBaseData { get; set; }

        [JsonIgnore]
        public byte[] _fileData { get; set; }
        public byte[] FileData
        {
            get { return _fileData; }
            set
            {
                if (string.IsNullOrEmpty(FileBaseData))
                {
                    FileBaseData = Convert.ToBase64String(value);
                }
                _fileData = value;
            }
        }
        public string DocumentURL { get; set; }
        public string FileRemark { get; set; }
    }
}
