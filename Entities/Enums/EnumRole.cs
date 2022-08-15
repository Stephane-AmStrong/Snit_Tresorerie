using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Enums
{
    public enum EnumRole
    {
        [Description("User")]
        User = 1,
        
        [Description("Admin")]
        Admin = 2,

        [Description("SuperAdmin")]
        SuperAdmin = 3,

    }
}
