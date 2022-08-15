using Entities.DataTransfertObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public record AppUserResponse
    {
        public string Id { get; set; }
        public string ImgLink { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public virtual TransactionResponse[] Transactions { get; set; }
    }
}
