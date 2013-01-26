using OptionRange.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OptionRange.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Home/OptionSummary

        public ActionResult OptionSummary(string shortName, string type, string strike, string expiry)
        {
            Option option = OptionRange.Utils.PageParser.getOption(shortName, type, strike, expiry);

            return PartialView("~/Views/OptionSummary.cshtml", option);
        }        
    }
}
