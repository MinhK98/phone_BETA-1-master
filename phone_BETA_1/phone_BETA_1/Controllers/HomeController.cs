using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using phone_BETA_1.Models;
using System.Data.Entity;

namespace phone_BETA_1.Controllers
{
    public class HomeController : Controller
    {
        phone_BETAEntities1 db = new phone_BETAEntities1();

        public ActionResult Index()
        {
            ViewBag.listProducts = db.SANPHAMs.ToList();
            return View();
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            SANPHAM spModel = new SANPHAM();

            using (phone_BETAEntities1 db = new phone_BETAEntities1())
            {
                spModel = db.SANPHAMs.Where(x => x.ID_SP == id).FirstOrDefault();
            }

            return View(spModel);
        }
    }
}