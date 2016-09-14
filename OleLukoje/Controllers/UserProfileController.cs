using OleLukoje.Filters;
using OleLukoje.Helpers.Page;
using OleLukoje.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace OleLukoje.Controllers
{
    public class UserProfileController : Controller
    {
        OleLukojeContext db = new OleLukojeContext();

        [HttpGet]
        [AllowAnonymous]
        [InitializeSimpleMembership]
        public ActionResult UserProfile(string userName)
        {
            userName = userName ?? WebSecurity.CurrentUserName;
            ViewBag.UserName = userName;
            using (UsersContext usdb = new UsersContext())
            {
                if (!usdb.UserProfiles.Any(u => u.UserName == userName))
                {
                    return View("UserNotFound");
                }
            }
            return View("UserProfile");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult _UserProfileInfo(string userName)
        {
            UserProfile userProfile = new UserProfile();
            using (UsersContext usdb = new UsersContext())
            {
                userProfile = usdb.UserProfiles.Single(u => u.UserName == userName);
                ViewBag.AdsCount = userProfile.Ads.Count;
            }

            ViewBag.Mobile = Request.Browser.IsMobileDevice;

            return PartialView("_UserProfileInfoPartial", userProfile);
        }

        [HttpGet]
        [AllowAnonymous]
        [InitializeSimpleMembership]
        public ActionResult _UserProfileApplication(string userName, int pageNumber = 1)
        {
            ViewBag.UserName = userName;
            List<Application> applications = new List<Application>();
            using (OleLukojeContext db = new OleLukojeContext())
            {
                applications = db.Applications.Where(application => application.Ad.UserProfile.UserName == userName).ToList();
            }
            return PartialView("_UserProfileApplicationPartial", new Page<Application>(applications, pageNumber, 3));
        }

        [HttpGet]
        [AllowAnonymous]
        [InitializeSimpleMembership]
        public PartialViewResult _UserProfileListAds(string userName, int pageNumber = 1)
        {
            ViewBag.UserName = userName;
            List<Ad> ads = db.Ads.Where(ad => ad.UserProfile.UserName == userName).ToList();
            ads.Reverse();
            return PartialView("_UserProfileListAdsPartial", new Page<Ad>(ads, pageNumber, 3));
        }

        [HttpPost]
        public PartialViewResult DeleteAd(int idAd)
        {
            lock (db)
            {
                Ad deleteAd = db.Ads.Single(ad => ad.Id == idAd);
                db.Ads.Remove(deleteAd);
                db.SaveChanges();
            }
            List<Ad> ads = db.Ads.Where(ad => ad.UserProfile.UserName == User.Identity.Name).ToList();
            return PartialView("_UserProfileListAdsPartial", new Page<Ad>(ads, 1, 3));
        }

        [HttpPost]
        public PartialViewResult RefreshAd(int idAd)
        {
            lock (db)
            {
                Ad refreshAd = db.Ads.Single(ad => ad.Id == idAd);
                refreshAd.StateAd = State.Active;
                db.SaveChanges();
            }
            List<Ad> ads = db.Ads.Where(ad => ad.UserProfile.UserName == User.Identity.Name).ToList();
            return PartialView("_UserProfileListAdsPartial", new Page<Ad>(ads, 1, 3));
        }
    }
}
