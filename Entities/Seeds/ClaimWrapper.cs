using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Seeds
{
    public class ClaimWrapper : Claim
    {
        public ClaimWrapper(string policyName, string type, string value) : base(type, value)
        {
            PolicyName = policyName;
        }

        public string PolicyName { get; set; }
    }
}
