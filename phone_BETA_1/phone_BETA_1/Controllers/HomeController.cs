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

        // 1. Chức năng: HIỂN THỊ SẢN PHẨM (NÓI CHUNG)
        public ActionResult Index()
        {
            ViewBag.listProducts = db.SANPHAMs.ToList();
            return View();
        }

        // 2. Chức năng: HIỂN THỊ CHI TIẾT SẢN PHẨM
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

        // 3. Chức năng: HIỂN THỊ SẢN PHẨM THEO NHÓM HÀNG
        public ActionResult TimKiemSanPham_NhomHang()
        {
            phone_BETAEntities1 entities = new phone_BETAEntities1();
            return View(entities.Search_SP_NH(""));
        }
        [HttpPost]
        public ActionResult TimKiemSanPham_NhomHang(string NhomHang)
        {
            phone_BETAEntities1 entities = new phone_BETAEntities1();
            return View(entities.Search_SP_NH(NhomHang));
        }

        // 4. Chức năng: HIỂN THỊ SẢN PHẨM THEO LOẠI HÀNG
        public ActionResult TimKiemSanPham_LoaiHang()
        {
            phone_BETAEntities1 entities = new phone_BETAEntities1();
            return View(entities.Search_SP_LH(""));
        }
        [HttpPost]
        public ActionResult TimKiemSanPham_LoaiHang(string LoaiHang)
        {
            phone_BETAEntities1 entities = new phone_BETAEntities1();
            return View(entities.Search_SP_LH(LoaiHang));
        }

        // 5. Chức năng: HIỂN THỊ SẢN PHẨM THEO HÃNG SẢN XUẤT
        public ActionResult TimKiemSanPham_HSX()
        {
            phone_BETAEntities1 entities = new phone_BETAEntities1();
            return View(entities.Search_SP_HSX(""));
        }
        [HttpPost]
        public ActionResult TimKiemSanPham_HSX(string HangSX)
        {
            phone_BETAEntities1 entities = new phone_BETAEntities1();
            return View(entities.Search_SP_HSX(HangSX));
        }

        // 6. Chức năng: TÌM KIẾM SẢN PHẨM (NÓI CHUNG)
        public ActionResult TimKiemSanPham_TenSP()
        {
            phone_BETAEntities1 entities = new phone_BETAEntities1();
            return View(entities.Search_SP_Name(""));
        }

        [HttpPost]
        public ActionResult TimKiemSanPham_TenSP(string Name_SP)
        {
            phone_BETAEntities1 entities = new phone_BETAEntities1();
            return View(entities.Search_SP_Name(Name_SP));
        }
    }
}