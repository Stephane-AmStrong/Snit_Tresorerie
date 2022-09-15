using Entities.DataTransfertObjects;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class AppUserViewModel : AppUserRequest
    {
        //public IFormFile File { get; set; }
        public virtual IFormFile? ImgFile { get; set; }
    }
}
