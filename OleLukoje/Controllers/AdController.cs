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
                    db.Ads.Add(new Ad { Header = ad.Header, Description = ad.Description, Price = ad.Price, Categories = categories, StateAd = State.Active, SpecialAd = false, UserId = userId, DateAd = DateTime.Now.Date });
                    db.SaveChanges();
                }
            }
            return View("ListAdView");
        }

        [HttpGet]
        public ActionResult SingleAdView(int id)
        {
            Ad ad = db.Ads.Single(t => t.Id == id);
            ViewBag.Ad = ad;
            ViewBag.Categories = ad.Categories;
            using (UsersContext uc = new UsersContext())
            {
                int i = ViewBag.Ad.UserId;
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

        [HttpGet]
        public ActionResult _SearchAd()
        {
            ViewBag.Categories = db.Categories.ToList();
            return PartialView("_SearchAdPartial");
        }

        /// <summary>
        /// ads - partial
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [InitializeSimpleMembership]
        public ActionResult _ListAd(List<string> categories, string userName, State? state, bool? specialAd, int? minPrice, int? maxPrice)
        {
            List<Ad> ads = db.Ads.ToList();
            if (categories != null)
            {
                ads = ads.Where(ad => ad.Categories.Where(category => categories.Contains(category.Name)).Count() != 0).ToList();
            }
            if (userName != null)
            {
                ads = ads.Where(ad => ad.UserId == WebSecurity.GetUserId(userName)).ToList();
            }
            if (state != null)
            {
                ads = ads.Where(ad => ad.StateAd == state).ToList();
            }
            if (specialAd != null && specialAd != false)
            {
                ads = ads.Where(ad => ad.SpecialAd == specialAd).ToList();
            }
            ViewBag.Title = userName != null ? userName + " ads" : "All ads";
            ViewBag.UserName = User.Identity.Name;
            ads.Reverse();
            return PartialView("_ListAdPartial", ads);
        }

        [HttpGet]
        [InitializeSimpleMembership]
        public ActionResult _ListAdSearch(string input)
        {
            input = input.ToUpper();
            List<Ad> ads = db.Ads.Where(ad => ad.Description.ToUpper().Contains(input) || ad.Header.ToUpper().Contains(input) || ad.UserProfile.UserName.ToUpper() == input).ToList();
            ads.Reverse();
            ViewBag.Title = "Find ads:";
            ViewBag.UserName = User.Identity.Name;
            return PartialView("_ListAdPartial", ads);
        }

        [HttpGet]
        public ActionResult _GetMessages(int idAd)
        {
            lock (db)
            {
                ViewBag.Messages = db.Messages.ToList();
                ViewBag.IdAd = idAd;
            }
            return PartialView("_MessagesPartial");
        }

        [HttpPost]
        [Authorize]
        [InitializeSimpleMembership]
        public ActionResult AddMessages(int idAd, string text)
        {
            lock (db)
            {
                Message message = new Message { Text = text, DateTimeMessage = DateTime.Now, AdId = idAd, UserId = WebSecurity.GetUserId(User.Identity.Name), UserName = User.Identity.Name };
                lock (db)
                {
                    db.Messages.Add(message);
                    db.SaveChanges();
                }
                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
