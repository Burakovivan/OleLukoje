using OleLukoje.Filters;
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
            }
            return PartialView("_UserProfileInfoPartial", userProfile);
        }

        [HttpGet]
        [AllowAnonymous]
        [InitializeSimpleMembership]
        public ActionResult _UserProfileApplication(string userName)
        {
            List<Application> applications = new List<Application>();
            using (OleLukojeContext db = new OleLukojeContext())
            {
                applications = db.Applications.Where(application => application.Ad.UserProfile.UserName == userName).ToList();
            }
            return PartialView("_UserProfileApplicationPartial", applications);
        }

        [HttpGet]
        [AllowAnonymous]
        [InitializeSimpleMembership]
        public PartialViewResult _UserProfileAds(string userName)
        {
            List<Ad> ads = db.Ads.Where(ad => ad.UserProfile.UserName == userName).ToList();
            ads.Reverse();
            return PartialView("_ListAdsPartial", ads);
        }
    }
}
