namespace Models
{
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    
    public class UploadFileRequest
    {
        public List<IFormFile> Files { get; set; }

        public List<string> MetadataValues { get; set; }
    }
}
