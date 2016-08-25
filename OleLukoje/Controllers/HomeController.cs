using OleLukoje.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OleLukoje.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string input)
        {
            ViewBag.Search = input;
            return View("Index");
        }
    }
}
