using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OleLukoje.Helpers.Page
{
    public class Page<PageType>
    {
        public IEnumerable<PageType> Items { get; set; }
        public PageInfo PageInfo { get; set; }

        public Page(IEnumerable<PageType> inputItems, int pageNumber = 1, int pageSize = 3)
        {
            PageInfo = new PageInfo(pageNumber, pageSize, inputItems.Count());
            Items = inputItems.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }
}