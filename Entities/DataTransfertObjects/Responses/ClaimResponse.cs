using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Entities.DataTransfertObjects
{
    public record ClaimResponse
    {
        public bool IsSelected { get; set; }

        public string Type { get; set; }
        public string Issuer { get; set; }
        public string ValueType { get; set; }
        public string Value { get; set; }
    }
}
