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

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddAd()
        {
            List<SelectListItem> itemsCategories = new List<SelectListItem>();
            foreach (Category category in db.Categories)
            {
                itemsCategories.Add(new SelectListItem { Text = category.Name, Value = category.Id.ToString() });
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
                    db.Ads.Add(new Ad { Header = ad.Header, Description = ad.Description, Price = ad.Price, Categories = categories, StateAd = State.Active, SpecialAd = false, UserProfileId = userId });
                    db.SaveChanges();
                }
            }
            return View("ListAdView");
        }
    }
}
