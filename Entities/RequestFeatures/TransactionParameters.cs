using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.RequestFeatures
{
    public class TransactionParameters : QueryStringParameters
    {
        public TransactionParameters()
        {
            OrderBy = "nature";
        }

        public string OrganizedBy { get; set; }
        public Guid? OfCategoryId { get; set; }
        public Guid? OnTheBillId { get; set; }
        public string PublicOnly { get; set; }
        public bool AvailableOnly { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
