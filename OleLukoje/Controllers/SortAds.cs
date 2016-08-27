using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OleLukoje.Models
{
    public enum SortBy
    {
        byHeaderA,
        byHeaderZ,
        byPriceMin,
        byPriceMax,
        byDataNew,
        byDataOld
    }

    public class SortAds
    {
        private OleLukojeContext db = new OleLukojeContext();

        public static List<Ad> SortAdsBy(List<Ad> ads, SortBy sortBy)
        {
            switch (sortBy)
            {
                case SortBy.byHeaderA:
                    {
                        return ads.OrderBy(ad => ad.Header).ToList();
                    }
                case SortBy.byHeaderZ:
                    {
                        return ads.OrderByDescending(ad => ad.Header).ToList();
                    }
                case SortBy.byPriceMin:
                    {
                        return ads.OrderBy(ad => ad.Price).ToList();
                    }
                case SortBy.byPriceMax:
                    {
                        return ads.OrderByDescending(ad => ad.Price).ToList();
                    }
                case SortBy.byDataOld:
                    {
                        return ads.OrderBy(ad => ad.DateAd).ToList();
                    }
                case SortBy.byDataNew:
                    {
                        return ads.OrderByDescending(ad => ad.DateAd).ToList();
                    }
                default: return ads;
            }
        }
    }
}