using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.RequestFeatures
{
    public class ActorParameters : QueryStringParameters
    {
        public ActorParameters()
        {
            OrderBy = "lastname";
        }

    }
}
