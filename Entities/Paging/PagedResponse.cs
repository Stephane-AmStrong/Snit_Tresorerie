using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Paging
{
    public class PagedResponse<T>
    {
        public T Data { get; set; }
        public MetaData MetaData { get; set; }
        public int Spread { get; set; }
        public List<PagingLink> Links { get; set; }

        public PagedResponse(T Data, MetaData metaData, int spread)
        {
            this.Data = Data;
            Links = new List<PagingLink>();
            MetaData = metaData;
            Spread = spread;

            Links.Add(new PagingLink(MetaData.CurrentPage - 1, MetaData.HasPrevious, "Préc"));

            for (int i = 1; i <= MetaData.TotalPages; i++)
            {
                if (i >= MetaData.CurrentPage - Spread && i <= MetaData.CurrentPage + Spread)
                {
                    Links.Add(new PagingLink(i, true, i.ToString()) { Active = MetaData.CurrentPage == i });
                }
            }

            Links.Add(new PagingLink(MetaData.CurrentPage + 1, MetaData.HasNext, "Suiv"));
        }
    }
}
