using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.RequestFeatures
{
    public class SiteParameters : QueryStringParameters
    {
        public SiteParameters()
        {
            OrderBy = "name";
        }

        public string AddBy { get; set; }

    }
}
