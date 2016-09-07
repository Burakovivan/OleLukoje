using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OleLukoje.Helpers.Page
{
    public class PageInfo
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int CountItems { get; set; }
        public int CountPages
        {
            get { return (int)Math.Ceiling((decimal)CountItems / PageSize); }
        }

        public PageInfo(int pageNumber, int pageSize, int countItems)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            CountItems = countItems;
        }
    }
}