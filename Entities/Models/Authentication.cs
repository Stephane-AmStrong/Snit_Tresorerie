using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Authentication
    {
        public Dictionary<string, string> UserInfo { get; set; }
        public virtual AppUser AppUser { get; set; }
        public string Token { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> ErrorDetails { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
