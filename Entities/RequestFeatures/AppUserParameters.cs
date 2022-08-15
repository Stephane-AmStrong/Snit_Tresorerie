using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class AppUserParameters : QueryStringParameters
    {
        public AppUserParameters()
        {
            OrderBy = "lastname";
        }
        public bool DisplayOrganisatorOnly { get; set; }
        public string WithRoleName { get; set; }
        public string AppUserId { get; set; }
        public string Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}