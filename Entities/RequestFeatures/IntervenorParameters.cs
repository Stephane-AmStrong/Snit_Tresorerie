using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.RequestFeatures
{
    public class IntervenorParameters : QueryStringParameters
    {
        public IntervenorParameters()
        {
            OrderBy = "lastname";
        }

    }
}
