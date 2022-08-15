using Entities.RequestFeatures;
using Entities.ViewModels;
using System.Collections.Generic;


namespace Snit_Tresorerie_WebApp.Wrapper
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

            Links.Add(new PagingLink(MetaData.CurrentPage - 1, MetaData.HasPrevious, "«"));

            for (int i = 1; i <= MetaData.TotalPages; i++)
            {
                if (i >= MetaData.CurrentPage - Spread && i <= MetaData.CurrentPage + Spread)
                {
                    Links.Add(new PagingLink(i, true, i.ToString()) { Active = MetaData.CurrentPage == i });
                }
            }

            Links.Add(new PagingLink(MetaData.CurrentPage + 1, MetaData.HasNext, "»"));
        }
    }
}
