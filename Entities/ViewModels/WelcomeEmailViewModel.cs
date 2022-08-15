using Entities.DataTransfertObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class WelcomeEmailViewModel
    {
        public AppUserResponse User { get; set; }
        public string ConfirmationLink { get; set; }
    }
}
