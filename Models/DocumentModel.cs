using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiUpload.Models
{
    [Table("Document")]
    public class DocumentModel
    {
        [Column("ID")]
        public int DocumentID { get; set; }
        [Column("FileName")]
        public string FileName { get; set; }
        [Column("ContentType")]
        public string ContentType { get; set; }
        [Column("FileSize")]
        public long? FileSize { get; set; }
    }
}
