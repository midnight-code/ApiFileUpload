using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Drawing;
using System.Reflection.Metadata;
using WebApiUpload.Models;
using WebApiUpload.Repositor;

namespace WebApiUpload.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IDocumentRepositor _documentRepositor;
        public DocumentsController(IWebHostEnvironment environment, IDocumentRepositor documentRepositor)
        {
            _documentRepositor = documentRepositor;
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }


        [HttpPost("/uploadfile", Name = "UploadeFile")]
        public async Task<ActionResult> UploadFiles(IFormFile file)
        {
            int idDocument = 0;
            
            var httpReqest = HttpContext.Request;
            var rootPath = Path.Combine(_environment.ContentRootPath, "Uploades", "Documents");
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);
            var path = Path.Combine(rootPath, file.FileName);

            MiniImage(file, rootPath);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                var document = new DocumentModel()
                {
                    FileName = file.FileName,
                    ContentType = file.ContentType,
                    FileSize = file.Length
                };
                await file.CopyToAsync(stream);
                idDocument = _documentRepositor.CreateDocument(document);
            }
            return Ok(new { idDocument });
        }


        [HttpPost("/uploadedocument", Name = "UploadeDocument")]
        public async Task<IActionResult> Post()
        {
            var id = 0;
            var document = new DocumentModel();
            try
            {
                var httpRequest = HttpContext.Request;

                if (httpRequest.Form.Files.Count > 0)
                {
                    foreach (var file in httpRequest.Form.Files)
                    {
                        var rootPath = Path.Combine(_environment.ContentRootPath, "Uploades", "Documents");

                        if (!Directory.Exists(rootPath))
                            Directory.CreateDirectory(rootPath);

                        MiniImage(file, rootPath);

                        using (var memoryStream = new MemoryStream())
                        {
                            await file.CopyToAsync(memoryStream);
                            System.IO.File.WriteAllBytes(Path.Combine(rootPath, file.FileName), memoryStream.ToArray());

                                                       
                            document = new DocumentModel()
                            {
                                FileName = file.FileName,
                                ContentType = file.ContentType,
                                FileSize = file.Length
                            };
                            if (document.ContentType == null)
                                return BadRequest(document.ContentType);
                            id = _documentRepositor.CreateDocument(document);
                        }

                        return Ok(document.DocumentID);
                    }
                }
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }

            return BadRequest("gjcktlybq");
        }

        [HttpPost("/DownloadDocument/{id}")]
        public async Task<ActionResult> DownLoad(int id)
        {
            var provider = new FileExtensionContentTypeProvider();
            var document = _documentRepositor.GetDocumetByID(id).FirstOrDefault();
            if (document == null)
            {
                document = _documentRepositor.GetDocumetByID(1).FirstOrDefault();
            }
            var rootPath = Path.Combine(_environment.ContentRootPath, "Uploades", "Documents", $"mini_{document.FileName}");
            string contentType;
            bool i = provider.TryGetContentType(rootPath, out contentType);
            if (!provider.TryGetContentType(rootPath, out contentType))
            {
                contentType = document.ContentType;
            }

            byte[] filebyte;
            if (System.IO.File.Exists(rootPath))
            {
                filebyte = System.IO.File.ReadAllBytes(rootPath);
            }
            else
                return NotFound();

            return File(filebyte, contentType, document.FileName);
        }
#pragma warning disable CA1416
        /// <summary>
        /// Сохранаяем маленькую картинку, для дальнейшей работы с ним.
        /// </summary>
        /// <param name="file">Изображение</param>
        /// <param name="rootPath">Дериктория для сохранения</param>
        private void MiniImage(IFormFile file, string rootPath)
        {
            int width = 400, height = 400;
            Image image = Image.FromStream(file.OpenReadStream(), true, true);
            var newImage = new Bitmap(width, height);
            using (var a = Graphics.FromImage(newImage))
            {
                a.DrawImage(image, 0, 0, width, height);
                var path2 = Path.Combine(rootPath, $"mini_{file.FileName}");
                newImage.Save(path2);
            }
        }
#pragma warning restore CA1416
    }
}
