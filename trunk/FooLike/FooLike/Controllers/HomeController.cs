using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FooLike.Models;

namespace FooLike.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        [HttpGet]
        public ActionResult Index()
        {
            return Json(new DBDataContext().Accounts.ToList(), JsonRequestBehavior.AllowGet);
        }

    }
}
