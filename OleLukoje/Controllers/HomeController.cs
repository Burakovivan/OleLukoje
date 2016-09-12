using OleLukoje.Filters;
using OleLukoje.Models;
using OleLukoje.Helpers.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OleLukoje.Controllers
{
    public class HomeController : Controller
    {
        OleLukojeContext db = new OleLukojeContext();

        public int GetMinPrice(List<Ad> ads)
        {
            return ads == null || ads.Count == 0 ? 0 : ads.Min(ad => ad.Price);
        }

        public int GetMaxPrice(List<Ad> ads)
        {
            return ads == null || ads.Count == 0 ? 0 : ads.Max(ad => ad.Price);
        }

        public ActionResult Index(string input)
        {
            ViewBag.Search = input;
            return View("Index");
        }

        [HttpGet]
        public ActionResult _ListAds(FilterAd filter, SortBy? sortBy, int pageNumber = 1, int pageSize = 1, byte tiled=0)
        {
            List<Ad> ads = db.Ads.Where(ad => ad.StateAd == State.Active).ToList();
            if (ads.Count != 0)
            {
                ads = SortFilterAds.SortAdsBy(SortFilterAds.Filter(ads, filter), sortBy ?? SortBy.byDataNew);
            }
            ViewBag.MinPrice = GetMinPrice(ads);
            ViewBag.MaxPrice = GetMaxPrice(ads);
            ViewBag.Tiled = tiled;
            return PartialView("_ListAdsPartial", new Page<Ad>(ads, pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult _FilterAds()
        {
            ViewBag.Categories = db.Categories.ToList();
            return PartialView("_FilterAdsPartial");
        }

        /// <summary>
        /// Search by line
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _Search(string input)
        {
            input = input.ToUpper();
            List<Ad> ads = db.Ads.Where(ad => ad.Description.ToUpper().Contains(input) || ad.Header.ToUpper().Contains(input) || ad.UserProfile.UserName.ToUpper() == input).ToList();
            ads.Reverse();
            ViewBag.MinPrice = GetMinPrice(ads);
            ViewBag.MaxPrice = GetMaxPrice(ads);
            return PartialView("_ListAdsPartial", new Page<Ad>(ads, 1, 1));
        }
    }
}
