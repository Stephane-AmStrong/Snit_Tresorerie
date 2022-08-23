using Entities.RequestFeatures;
using System.Collections.Generic;

namespace Entities.Paging
{
    public class Pagination
    {
        public MetaData MetaData { get; set; }
        public int Spread { get; set; }
        public int SelectedPage { get; set; }
        public List<PagingLink> Links { get; set; }


        public Pagination(MetaData metaData, int spread/*, int selectedPage, List<PagingLink> links*/)
        {
            MetaData = metaData;
            Spread = spread;
            //SelectedPage = selectedPage;
            //Links = links;
        }
    }
}
