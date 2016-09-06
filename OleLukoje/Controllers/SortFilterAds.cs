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

    public class SortFilterAds
    {
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

        public static List<Ad> Filter(List<Ad> ads, FilterAd filter)
        {
            if (filter.Categories != null)
            {
                ads = ads.Where(ad => ad.Categories.Where(category => filter.Categories.Contains(category.Name)).Count() != 0).ToList();
            }
            if (filter.SpecialAd != false)
            {
                ads = ads.Where(ad => ad.SpecialAd == filter.SpecialAd).ToList();
            }
            if (filter.MinPrice != null)
            {
                ads = ads.Where(ad => ad.Price >= filter.MinPrice).ToList();
            }
            if (filter.MaxPrice != null && filter.MaxPrice != 0)
            {
                ads = ads.Where(ad => ad.Price <= filter.MaxPrice).ToList();
            }
            return ads;
        }
    }
}