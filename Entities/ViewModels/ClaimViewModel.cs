using System.Security.Claims;

namespace Entities.ViewModels
{
    public class ClaimViewModel : Claim
    {
        public ClaimViewModel(string policyName, string type, string value) : base(type, value)
        {
            PolicyName = policyName;
        }

        public string PolicyName { get; set; }
    }
}
