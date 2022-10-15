using Microsoft.AspNetCore.Authentication;
using System.Reflection.Metadata;
using WebApiUpload.Data;
using WebApiUpload.Models;

namespace WebApiUpload.Repositor
{
    public class DocumentRepositor : IDocumentRepositor
    {
        private readonly BaseContext _context;

        public DocumentRepositor(BaseContext context) => _context = context;

        public IEnumerable<DocumentModel> GetDocumetByID(int docID)
        {
            return _context.Documents.Where(id => id.DocumentID == docID);
        }

        public int CreateDocument(DocumentModel document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }
            _context.Documents.Add(document);
            _context.SaveChanges();
            return document.DocumentID;
        }
    }
} 