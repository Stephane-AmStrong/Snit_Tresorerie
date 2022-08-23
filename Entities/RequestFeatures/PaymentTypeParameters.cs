using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.RequestFeatures
{
    public class PaymentTypeParameters : QueryStringParameters
    {
        public PaymentTypeParameters()
        {
            OrderBy = "name";
        }

        public string AddBy { get; set; }

    }
}
