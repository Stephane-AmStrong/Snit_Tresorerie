using Entities.DataTransfertObjects;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class HomeViewModel
    {
        public virtual AppUserViewModel AppUserViewModel { get; set; }
        public virtual LoginRequest LoginRequest { get; set; }
        public virtual AppUserRequest AppUserRequest { get; set; }
    }
}
