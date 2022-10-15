using System.Reflection.Metadata;
using WebApiUpload.Models;

namespace WebApiUpload.Repositor
{
    public interface IDocumentRepositor
    {
        IEnumerable<DocumentModel> GetDocumetByID(int docID);
        public int CreateDocument(DocumentModel document);
    }
}
