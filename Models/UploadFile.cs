namespace Models
{
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;

    public class UploadFile
    {
        public string Id { get; set; }

        public IFormFile File { get; set; }

        public Dictionary<string, object> Metadata { get; set; }

        public string Type { get; set; }

        public string Base64 { get; set; }

        public string FileName { get; set; }

        public byte[] Bytes { get; set; }
    }
}
