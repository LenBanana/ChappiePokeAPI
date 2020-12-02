using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DTOModels
{
    public class FileUploadRequest
    {
        public List<IFormFile> Files { get; set; }
        public string Param { get; set; }

    }
}
