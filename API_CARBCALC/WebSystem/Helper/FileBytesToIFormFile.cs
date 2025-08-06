using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace WebSystem.Helper
{
    public class FileBytesToIFormFile : IFormFile
    {
        public FileBytesToIFormFile(byte[] fileBytes, string fileName)
        {
            FileBytes = fileBytes;
            FileName = fileName;
            ContentType = "image/jpeg";
        }

        public string ContentType { get; set; }
        public string ContentDisposition { get; }
        public IHeaderDictionary Headers { get; }
        public long Length { get { return FileBytes.Length; } }
        public string Name { get; set; }
        public string FileName { get; set; }

        public Stream OpenReadStream()
        {
            return new MemoryStream(FileBytes);
        }

        public void CopyTo(Stream target)
        {
            throw new System.NotImplementedException();
        }

        public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        private byte[] FileBytes;
    }

    //IFormFile formFile = new FileBytesToIFormFile(fileBytes, fileName);
}
