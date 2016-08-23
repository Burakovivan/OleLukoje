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

        public void Upload(IEnumerable<HttpPostedFileBase> uploads, int adId)
        {
            using (OleLukojeContext db = new OleLukojeContext())
            {
                foreach (var file in uploads)
                {
                    if (file != null)
                    {
                        // получаем путь к файлу
                        string filePath = "~/Files/" + System.IO.Path.GetFileName(file.FileName);
                        // сохраняем файл в папку Files в проекте
                        file.SaveAs(Server.MapPath(filePath));
                        lock (db)
                        {
                            db.Files.Add(new File { AdId = adId, Path = filePath });
                            db.SaveChanges();
                        }
                    }
                }
            }
        }

        [HttpGet]
        public ActionResult AddAd()
        {
            List<SelectListItem> itemsCategories = new List<SelectListItem>();
            foreach (Category category in db.Categories)
            {
                itemsCategories.Add(new SelectListItem
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                });
            }
            ViewBag.Category = itemsCategories;
            return View("AddAdView");
        }

        [HttpGet]
        public ActionResult SingleAdView(int id)
        {
            if (!db.Ads.Any(t => t.Id == id))
            {
                return View("AddNotFound");
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
            return View("SingleAdView");
        }

        [HttpGet]
        public ActionResult ListAds()
        {
            return View("ListAdsView");
        }

        [HttpGet]
        public ActionResult _FilterAds()
        {
            ViewBag.Categories = db.Categories.ToList();
            return PartialView("_FilterAdsPartial");
        }

        [HttpGet]
        public ActionResult _SearchAds()
        {
            return PartialView("_SearchAdsPartial");
        }

        /// <summary>
        /// All/user`s ads
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [InitializeSimpleMembership]
        public ActionResult _ListAds(string userName)
        {
            int userId = userName != null ? WebSecurity.GetUserId(userName) : -1;
            List<Ad> ads = userId != -1 ?
                db.Ads.Where(ad => ad.UserId == userId).ToList() :
                db.Ads.Where(ad => ad.StateAd == State.Active).ToList();
            ads.Reverse();
            ViewBag.MyAds = userName == User.Identity.Name;
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
            return PartialView("_ListAdsPartial", ads);
        }

        /// <summary>
        /// Search by line
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [InitializeSimpleMembership]
        public ActionResult _Search(string input)
        {
            input = input.ToUpper();
            List<Ad> ads = db.Ads.Where(ad => ad.Description.ToUpper().Contains(input) || ad.Header.ToUpper().Contains(input) || ad.UserProfile.UserName.ToUpper() == input).ToList();
            ads.Reverse();
            ViewBag.MyAds = false;
            return PartialView("_ListAdsPartial", ads);
        }

        [HttpGet]
        public ActionResult _GetComments(int idAd)
        {
            lock (db)
            {
                ViewBag.Comments = db.Comments.Where(comm => comm.AdId == idAd).ToList();
                ViewBag.IdAd = idAd;
            }
            return PartialView("_CommentsPartial");
        }

        [HttpPost]
        [InitializeSimpleMembership]
        public RedirectToRouteResult AddAd(List<string> SelectCategory, Ad ad, IEnumerable<HttpPostedFileBase> uploads)
        {
            if (ad != null)
            {
                lock (db)
                {
                    List<Category> categories = db.Categories.Where(category => SelectCategory.Contains(category.Name)).ToList();
                    int userId = (int)WebSecurity.CurrentUserId;
                    int adId = db.Ads.Count() + 1;
                    db.Ads.Add(new Ad
                    {
                        Header = ad.Header,
                        Description = ad.Description,
                        Price = ad.Price,
                        Categories = categories,
                        StateAd = State.Active,
                        SpecialAd = false,
                        UserId = userId,
                        DateAd = DateTime.Now.Date
                    });
                    db.SaveChanges();
                    Upload(uploads, adId);
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
        [Authorize]
        [InitializeSimpleMembership]
        public ActionResult AddComment(int idAd, string text)
        {
            Comment comment = new Comment
            {
                Text = text,
                DateTimeComment = DateTime.Now,
                AdId = idAd,
                UserName = User.Identity.Name
            };
            lock (db)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
            }
            return Json(comment, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [InitializeSimpleMembership]
        public ActionResult AddApplication(Application application)
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
