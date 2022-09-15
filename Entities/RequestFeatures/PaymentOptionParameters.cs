using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.RequestFeatures
{
    public class PaymentOptionParameters : QueryStringParameters
    {
        public PaymentOptionParameters()
        {
            OrderBy = "name";
        }

        public string AddBy { get; set; }

    }
}
