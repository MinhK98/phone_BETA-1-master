using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using phone_BETA_1.Models;
using System.Data.Entity;
using System.IO;

namespace phone_BETA_1.Controllers
{
    public class NHANVIENController : Controller
    {
        // I. DBO.SANPHAM:

        //// 1. Chức năng: XEM SẢN PHẨM
        public ActionResult SanPham()
        {
            using (phone_BETAEntities1 db = new phone_BETAEntities1())
            {
                return View(db.SANPHAMs.ToList());
            }
        }
        //// 2. Chức năng: XEM CHI TIẾT SẢN PHẨM
        public ActionResult ChiTietSanPham(int id)
        {
            using (phone_BETAEntities1 db = new phone_BETAEntities1())
            {
                return View(db.SANPHAMs.Where(x => x.ID_SP == id).FirstOrDefault());
            }
        }
        //// 3. Chức năng: THÊM MỚI SẢN PHẨM
        [HttpGet]
        public ActionResult ThemSanPham(int id = 0)
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemSanPham(SANPHAM sanpham, FormCollection frc)
        {
            string fileName = Path.GetFileNameWithoutExtension(sanpham.ImageFile.FileName);
            string extension = Path.GetExtension(sanpham.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            sanpham.Photo = "~/Images/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
            sanpham.ImageFile.SaveAs(fileName);
            using (phone_BETAEntities1 db = new phone_BETAEntities1())
            {
                db.SANPHAMs.Add(sanpham);
                db.SaveChanges();
            }
            ModelState.Clear();
            return View();
        }
        //// 4. Chức năng: XÓA SẢN PHẨM
        public ActionResult XoaSanPham(int id)
        {
            using (phone_BETAEntities1 db = new phone_BETAEntities1())
            {
                return View(db.SANPHAMs.Where(x => x.ID_SP == id).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult XoaSanPham(int id, FormCollection collection)
        {
            try
            {
                using (phone_BETAEntities1 db = new phone_BETAEntities1())
                {
                    SANPHAM sanpham = db.SANPHAMs.Where(x => x.ID_SP == id).FirstOrDefault();
                    db.SANPHAMs.Remove(sanpham);
                    db.SaveChanges();
                }
                return RedirectToAction("SanPham", "NHANVIEN");
            }
            catch
            {
                return View();
            }
        }
        //// 5. Chức năng: SỬA SẢN PHẨM
        public ActionResult SuaSanPham(int id)
        {
            using (phone_BETAEntities1 db = new phone_BETAEntities1())
            {
                return View(db.SANPHAMs.Where(x => x.ID_SP == id).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult SuaSanPham(int id, SANPHAM sanpham)
        {
            try
            {
                using (phone_BETAEntities1 db = new phone_BETAEntities1())
                {
                    db.Entry(sanpham).State = EntityState.Modified;
                    db.SaveChanges();
                }
                // TODO: Add update logic here

                return RedirectToAction("SanPham", "NHANVIEN");
            }
            catch
            {
                return View();
            }
        }
        //-----------------------------//

        // II. DBO.KHACHHANG:

        //// 1. Chức năng: XEM VÀ TÌM KIẾM KHÁCH HÀNG
        public ActionResult KhachHang()
        {
            phone_BETAEntities1 entities = new phone_BETAEntities1();
            return View(entities.Search_Customers(""));
        }

        [HttpPost]
        public ActionResult KhachHang(string NameCustomer)
        {
            phone_BETAEntities1 entities = new phone_BETAEntities1();
            return View(entities.Search_Customers(NameCustomer));
        }

        //// 2. Chức năng: XEM CHI TIẾT KHÁCH HÀNG
        public ActionResult ChiTietKhachHang(int id)
        {
            using (phone_BETAEntities1 db = new phone_BETAEntities1())
            {
                return View(db.KHACHHANGs.Where(x => x.ID_Customer == id).FirstOrDefault());
            }
        }
        //-----------------------------//

        // III. DBO.HOADON:

        //// 1. Chức năng: XEM HÓA ĐƠN KHÁCH HÀNG
        public ActionResult HoaDon()
        {
            using (phone_BETAEntities1 db = new phone_BETAEntities1())
            {
                return View(db.HOADONs.ToList());
            }
        }
        //// 2. Chức năng: XEM CHI TIẾT HÓA ĐƠN
        public ActionResult ChiTietHoaDon(int id)
        {
            using (phone_BETAEntities1 db = new phone_BETAEntities1())
            {
                return View(db.CHITIET_HOADON.Where(x => x.ID_HD == id).ToList());
            }
        }
        //// 3. Chức năng: CHỈNH SỬA STATUS CỦA HÓA ĐƠN
        public ActionResult SuaHoaDon(int id)
        {
            using (phone_BETAEntities1 db = new phone_BETAEntities1())
            {
                return View(db.HOADONs.Where(x => x.ID_HD == id).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult SuaHoaDon(int id, HOADON hoadon)
        {
            try
            {
                using (phone_BETAEntities1 db = new phone_BETAEntities1())
                {
                    db.Entry(hoadon).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("HoaDon");
            }
            catch
            {
                return View();
            }
        }
        //// 4. Chức năng: TÌM KIẾM HÓA ĐƠN THEO NGÀY VÀ TÊN KH
        public ActionResult TimKiemHoaDon()
        {
            phone_BETAEntities1 entities = new phone_BETAEntities1();
            return View(entities.SearchHoaDon("", ""));
        }

        [HttpPost]
        public ActionResult TimKiemHoaDon(string NameCustomer, string DateBill)
        {
            phone_BETAEntities1 entities = new phone_BETAEntities1();
            return View(entities.SearchHoaDon(NameCustomer, DateBill));
        }

    }
}