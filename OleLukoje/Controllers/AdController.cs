using OleLukoje.Filters;
using OleLukoje.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace OleLukoje.Controllers
{
    public class AdController : Controller
    {
        //
        // GET: /Ad/
        OleLukojeContext db = new OleLukojeContext();

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddAd()
        {
            List<SelectListItem> itemsCategories = new List<SelectListItem>();
            lock (db)
            {
                foreach (Category category in db.Categories)
                {
                    itemsCategories.Add(new SelectListItem { Text = category.Name, Value = category.Id.ToString() });
                }
            }
            ViewBag.Category = itemsCategories;
            return View("AddAdView");
        }

        [HttpPost]
        [InitializeSimpleMembership]
        public ActionResult AddAd(List<string> SelectCategory, Ad ad)
        {
            if (ad != null)
            {
                List<Category> categories = db.Categories.Where(category => SelectCategory.Contains(category.Name)).ToList();
                int userId = (int)WebSecurity.CurrentUserId;
                lock (db)
                {
                    db.Ads.Add(new Ad { Header = ad.Header, Description = ad.Description, Price = ad.Price, Categories = categories, StateAd = State.Active, SpecialAd = false, UserId = userId });
                    db.SaveChanges();
                }
            }
            return View("ListAdView");
        }

        [HttpGet]
        public ActionResult SingleAdView(int id)
        {
            Ad n = db.Ads.Single(t => t.Id == id);
            ViewBag.Ad = n;
            ViewBag.Categories = n.Categories;
            using (UsersContext uc = new UsersContext())
            {
                int i = ViewBag.Ad.UserProfileId;
                ViewBag.UserName = uc.UserProfiles.Single(t => t.UserId == i).UserName;
            } 
            return View("SingleAdView");
        }

        /// <summary>
        /// Ads with search
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ListAd()
        {
            return View("ListAdView");
        }

        /// <summary>
        /// ads - partial
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [InitializeSimpleMembership]
        public ActionResult _ListAd(Category category, string userName, State? state, bool? specialAd)
        {
            List<Ad> ads = db.Ads.ToList();
            if (category.Name != null)
            {
                ads = ads.Where(ad => ad.Categories.Count(x => x.Name == category.Name) != 0).ToList();
            }
            if (userName != null)
            {
                ads = ads.Where(ad => ad.UserId == WebSecurity.GetUserId(userName)).ToList();
            }
            if (state != null)
            {
                ads = ads.Where(ad => ad.StateAd == state).ToList();
            }
            if (specialAd != null)
            {
                ads = ads.Where(ad => ad.SpecialAd == specialAd).ToList();
            }
            ViewBag.Title = userName != null ? userName + " ads" : "All ads";
            ViewBag.UserName = User.Identity.Name;
            ads.Reverse();
            return PartialView("_ListAdPartial", ads);
        }
    }
}
