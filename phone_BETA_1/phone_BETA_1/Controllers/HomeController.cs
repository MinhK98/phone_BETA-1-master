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
        phone_BETAEntities db = new phone_BETAEntities();

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
            using (phone_BETAEntities db = new phone_BETAEntities())
            {
                phone_BETAEntities entities = new phone_BETAEntities();
                db.SANPHAMs.Where(x => x.ID_SP == id).FirstOrDefault();
                return View(entities.ChiTietSanPham(id.ToString()).FirstOrDefault());
            }
        }

        // 3. Chức năng: HIỂN THỊ SẢN PHẨM THEO NHÓM HÀNG
        public ActionResult TimKiemSanPham_NhomHang()
        {
            phone_BETAEntities entities = new phone_BETAEntities();
            return View(entities.SanPham_SearchNH(""));
        }
        [HttpPost]
        public ActionResult TimKiemSanPham_NhomHang(string NhomHang)
        {
            phone_BETAEntities entities = new phone_BETAEntities();
            return View(entities.SanPham_SearchNH(NhomHang));
        }

        // 4. Chức năng: HIỂN THỊ SẢN PHẨM THEO LOẠI HÀNG
        public ActionResult TimKiemSanPham_LoaiHang()
        {
            phone_BETAEntities entities = new phone_BETAEntities();
            return View(entities.SanPham_SearchLH(""));
        }
        [HttpPost]
        public ActionResult TimKiemSanPham_LoaiHang(string LoaiHang)
        {
            phone_BETAEntities entities = new phone_BETAEntities();
            return View(entities.SanPham_SearchLH(LoaiHang));
        }

        // 5. Chức năng: HIỂN THỊ SẢN PHẨM THEO HÃNG SẢN XUẤT
        public ActionResult TimKiemSanPham_HSX()
        {
            phone_BETAEntities entities = new phone_BETAEntities();
            return View(entities.SanPham_SearchHSX(""));
        }
        [HttpPost]
        public ActionResult TimKiemSanPham_HSX(string HangSX)
        {
            phone_BETAEntities entities = new phone_BETAEntities();
            return View(entities.SanPham_SearchHSX(HangSX));
        }

        // 6. Chức năng: TÌM KIẾM SẢN PHẨM (NÓI CHUNG)
        public ActionResult TimKiemSanPham_TenSP()
        {
            phone_BETAEntities entities = new phone_BETAEntities();
            return View(entities.SanPham_SearchTenSP(""));
        }

        [HttpPost]
        public ActionResult TimKiemSanPham_TenSP(string Name_SP)
        {
            phone_BETAEntities entities = new phone_BETAEntities();
            return View(entities.SanPham_SearchTenSP(Name_SP));
        }
    }
}