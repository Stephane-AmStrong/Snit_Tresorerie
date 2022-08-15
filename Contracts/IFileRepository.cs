using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IFileRepository
    {
        public string FilePath { set; }

        public Task<string> UploadFile(IFormFile file);

        public Task DeleteFile(string filePath);
    }
}
