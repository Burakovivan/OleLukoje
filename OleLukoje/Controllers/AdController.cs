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

        public List<File> Upload(IEnumerable<HttpPostedFileBase> uploads, int adId)
        {
            List<File> files = new List<File>();
            foreach (var file in uploads)
            {
                if (file != null)
                {
                    string filePath = "/Files/" + adId + "-" + files.Count + System.IO.Path.GetExtension(file.FileName);
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
        public ActionResult _GetAdInformation(int idAd)
        {
            Ad curr = db.Ads.First(ad => ad.Id == idAd);
            ViewBag.photo = curr.Files.Any() ? curr.Files.First().Path : "/Files/default.png";
            return PartialView("_AdInformationPartial", curr);
        }

        [HttpGet]
        public ActionResult _GetReviews(int idAd)
        {
            List<Review> reviews = db.Reviews.Where(review => review.AdId == idAd).ToList();
            reviews.Reverse();
            ViewBag.AdId = idAd;
            
            return PartialView("_ReviewsPartial", reviews);
        }

        [HttpGet]
        public ActionResult _GetPhotos(int idAd)
        {
            List<string> photos = db.Files.Where(p => p.AdId.Equals(idAd)).Select(t => t.Path).ToList();
            return PartialView("_GetPhotosPartial", photos);
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
            return RedirectToAction("UserProfile", "UserProfile");
        }

        [HttpPost]
        [InitializeSimpleMembership]
        public ActionResult AddReview(Review review)
        {
            List<Review> reviews = new List<Review>();
            review.DateTimeReview = DateTime.Now;
            review.UserName = review.UserName == string.Empty ? "Anonim" : review.UserName;
            lock (db)
            {
                db.Reviews.Add(review);
                db.SaveChanges();
                reviews = db.Reviews.ToList();
                reviews.Reverse();
            }
            ViewBag.AdId = review.AdId;
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
