using Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class FileRepository : IFileRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string baseFolderPath;
        private string relativeFilePath;

        private string fileId;
        public string FilePath { set => fileId = value; }



        public FileRepository(IWebHostEnvironment webHostEnvironment, string filePath)
        {
            _webHostEnvironment = webHostEnvironment;
            this.baseFolderPath = $"{_webHostEnvironment.WebRootPath}/{filePath}";

            relativeFilePath = $"{filePath}";
        }



        public async Task DeleteFile(string relativeFilePath)
        {
            var fullPath = $"{_webHostEnvironment.WebRootPath}/{relativeFilePath}";

            if (File.Exists(fullPath))
            {
                await Task.Run(() => File.Delete(fullPath));
            }

        }

        public async Task<string> UploadFile(IFormFile file)
        {
            #region Try catch version
            /*
             
            try
            {
                if (!System.IO.Directory.Exists(baseFolderPath)) Directory.CreateDirectory(baseFolderPath);


                if (file.Length > 0)
                {
                    string extention = file.FileName.Split('.').Last();

                    var fullPath = $"{this.baseFolderPath}/{fileId}.{extention}";
                    var relativePath = $"{this.relativeFilePath}/{fileId}.{extention}";
                    using (var stream = File.Create(fullPath))
                    {
                        await file.CopyToAsync(stream);
                        await stream.FlushAsync();
                        return relativePath;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            } 

             */
            #endregion

            if (!System.IO.Directory.Exists(baseFolderPath)) Directory.CreateDirectory(baseFolderPath);


            if (file.Length > 0)
            {
                string extention = file.FileName.Split('.').Last();

                var fullPath = $"{this.baseFolderPath}/{fileId}.{extention}";
                var relativePath = $"{this.relativeFilePath}/{fileId}.{extention}";
                using (var stream = File.Create(fullPath))
                {
                    await file.CopyToAsync(stream);
                    await stream.FlushAsync();
                    return relativePath;
                }
            }
            return null;
        }
    }
}
