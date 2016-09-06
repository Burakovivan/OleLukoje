using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OleLukoje.Models
{
    public class FilterAd
    {
        public List<string> Categories { get; set; } 
        public bool SpecialAd { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
    }
}