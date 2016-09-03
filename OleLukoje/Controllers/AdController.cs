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
        OleLukojeContext db = new OleLukojeContext();

        private int GetMinPrice(List<Ad> ads)
        {
            return ads == null || ads.Count == 0 ? 0 : ads.Min(ad => ad.Price);
        }

        private int GetMaxPrice(List<Ad> ads)
        {
            return ads == null || ads.Count == 0 ? 0 : ads.Max(ad => ad.Price);
        }

        public List<File> Upload(IEnumerable<HttpPostedFileBase> uploads, int adId)
        {
            List<File> files = new List<File>();
            foreach (var file in uploads)
            {
                if (file != null)
                {
                    string filePath = "~/Files/" + adId + "-" + files.Count + System.IO.Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath(filePath));
                    files.Add(new File { AdId = adId, Path = filePath });
                }
            }
            return files;
        }

        [HttpGet]
        public ActionResult AddAd()
        {
            ViewBag.Categories = db.Categories.ToList();
            return View("AddAdView");
        }

        [HttpGet]
        public ActionResult SingleAdView(int id)
        {
            if (!db.Ads.Any(t => t.Id == id))
            {
                return View("Error");
            }
            Ad ad = db.Ads.Single(t => t.Id == id);
            ViewBag.Ad = ad;
            ViewBag.Categories = ad.Categories;
            using (UsersContext uc = new UsersContext())
            {
                int i = ViewBag.Ad.UserId;
                ViewBag.UserNameSeller = uc.UserProfiles.Single(t => t.UserId == i).UserName;
            }
            ViewBag.UserName = User.Identity.Name;
            ViewBag.CountReviews = ad.Reviews.Count;
            return View("SingleAdView");
        }

        [HttpGet]
        public ActionResult _FilterAds()
        {
            ViewBag.Categories = db.Categories.ToList();
            return PartialView("_FilterAdsPartial");
        }

        /// <summary>
        /// All/user`s ads
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [InitializeSimpleMembership]
        public ActionResult _ListAds(string userName, List<Ad> inputAds)
        {
            int userId = userName != null ? WebSecurity.GetUserId(userName) : -1;
            List<Ad> ads = userId != -1 ?
                db.Ads.Where(ad => ad.UserId == userId).ToList() :
                inputAds ?? db.Ads.Where(ad => ad.StateAd == State.Active).ToList();
            ads.Reverse();
            ViewBag.MyAds = userName == User.Identity.Name;
            ViewBag.MinPrice = GetMinPrice(ads);
            ViewBag.MaxPrice = GetMaxPrice(ads);
            return PartialView("_ListAdsPartial", ads);
        }

        /// <summary>
        /// Filter by parametrs
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [InitializeSimpleMembership]
        public ActionResult _Filter(List<string> categories, State? state, bool specialAd, int? minPrice, int? maxPrice)
        {
            List<Ad> ads = db.Ads.ToList();
            if (categories != null)
            {
                ads = ads.Where(ad => ad.Categories.Where(category => categories.Contains(category.Name)).Count() != 0).ToList();
            }
            if (state != null)
            {
                ads = ads.Where(ad => ad.StateAd == state).ToList();
            }
            if (specialAd != false)
            {
                ads = ads.Where(ad => ad.SpecialAd == specialAd).ToList();
            }
            if (minPrice != null)
            {
                ads = ads.Where(ad => ad.Price >= minPrice).ToList();
            }
            if (maxPrice != null)
            {
                ads = ads.Where(ad => ad.Price <= maxPrice).ToList();
            }
            ads.Reverse();
            ViewBag.MyAds = false;
            ViewBag.MinPrice = minPrice ?? GetMinPrice(ads);
            ViewBag.MaxPrice = maxPrice ?? GetMaxPrice(ads);
            return PartialView("_ListAdsPartial", ads);
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
            ViewBag.MyAds = false;
            ViewBag.MinPrice = GetMinPrice(ads);
            ViewBag.MaxPrice = GetMaxPrice(ads);
            return PartialView("_ListAdsPartial", ads);
        }

        [HttpGet]
        public ActionResult _Sort(List<int> AdsId, SortBy sortBy)
        {
            ViewBag.MyAds = false;
            if (AdsId != null)
            {
                List<Ad> ads = db.Ads.Where(ad => AdsId.Contains(ad.Id)).ToList();
                ViewBag.MinPrice = GetMinPrice(ads);
                ViewBag.MaxPrice = GetMaxPrice(ads);
                return PartialView("_ListAdsPartial", SortAds.SortAdsBy(ads, sortBy));
            }
            else
            {
                ViewBag.MinPrice = 0;
                ViewBag.MaxPrice = 0;
                return PartialView("_ListAdsPartial", null);
            }
        }

        [HttpGet]
        public ActionResult _GetAdInformation(int idAd)
        {
            return PartialView("_AdInformationPartial", (Ad)db.Ads.First(ad => ad.Id == idAd));
        }

        [HttpGet]
        public ActionResult _GetReviews(int idAd)
        {
            List<Review> reviews = db.Reviews.Where(review => review.AdId == idAd).ToList();
            reviews.Reverse();
            ViewBag.AdId = idAd;
            return PartialView("_ReviewsPartial", reviews);
        }

        [HttpPost]
        [InitializeSimpleMembership]
        public RedirectToRouteResult AddAd(List<string> selectCategories, Ad ad, IEnumerable<HttpPostedFileBase> uploads)
        {
            if (ad != null)
            {
                lock (db)
                {
                    int adId = db.Ads.Count() + 1;
                    List<Category> categories = db.Categories.Where(category => selectCategories.Contains(category.Name)).ToList();
                    db.Ads.Add(new Ad
                    {
                        Header = ad.Header,
                        Description = ad.Description,
                        Price = ad.Price,
                        Categories = categories,
                        StateAd = State.Active,
                        SpecialAd = false,
                        UserId = (int)WebSecurity.CurrentUserId,
                        DateAd = DateTime.Now.Date,
                        AdCharacteristics = ad.AdCharacteristics,
                        Files = Upload(uploads, adId)
                    });
                    db.SaveChanges();
                }
            }
            return RedirectToAction("UserProfile", "Account");
        }

        [HttpPost]
        public ActionResult DeleteAd(int idAd)
        {
            lock (db)
            {
                Ad deleteAd = db.Ads.Single(ad => ad.Id == idAd);
                db.Ads.Remove(deleteAd);
                db.SaveChanges();
            }
            return Json(idAd, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [InitializeSimpleMembership]
        public ActionResult AddReview(int idAd, string text, string dignity, string defects, string name, int? stars)
        {
            List<Review> reviews = new List<Review>();
            Review review = new Review
            {
                Text = text,
                Dignity = dignity,
                Defects = defects,
                Stars = stars,
                DateTimeReview = DateTime.Now,
                AdId = idAd,
                UserName = name == string.Empty ? "Anonim" : name
            };
            lock (db)
            {
                db.Reviews.Add(review);
                db.SaveChanges();
                reviews = db.Reviews.ToList();
                reviews.Reverse();
            }
            return PartialView("_ReviewsPartial", reviews);
        }

        [HttpPost]
        [InitializeSimpleMembership]
        public JsonResult AddApplication(Application application)
        {
            try
            {
                lock (db)
                {
                    application.DateTimeApplication = DateTime.Now.Date;
                    application.UserName = application.UserName == string.Empty ? "Anonim" : application.UserName;
                    db.Applications.Add(application);
                    db.SaveChanges();
                }
                return Json(new { Message = "Application was submitted." }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Message = "Error! The application was not sent." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
