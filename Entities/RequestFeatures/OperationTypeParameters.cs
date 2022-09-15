using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.RequestFeatures
{
    public class OperationTypeParameters : QueryStringParameters
    {
        public OperationTypeParameters()
        {
            OrderBy = "name";
        }

        public string Named { get; set; }

    }
}
