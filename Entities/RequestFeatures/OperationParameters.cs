using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.RequestFeatures
{
    public class OperationParameters : QueryStringParameters
    {
        public OperationParameters()
        {
            OrderBy = "name";
        }

        public string WithName { get; set; }
        public string OrganizedBy { get; set; }
        public Guid? OfCategoryId { get; set; }
        public Guid? OnTheBillId { get; set; }
        public string PublicOnly { get; set; }
        public bool AvailableOnly { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
