using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using phone_BETA_1.Models;
using System.Data.Entity;

namespace phone_BETA_1.Controllers
{
    public class KHACHHANGController : Controller
    {
        // 1. Chức năng: XEM HÓA ĐƠN THEO ID KHÁCH HÀNG
        public ActionResult HoaDon(int id)
        {
            using (phone_BETAEntities db = new phone_BETAEntities())
            {
                return View(db.HOADONs.Where(x => x.ID_Customer == id).ToList());
            }
        }
        // 2. Chức năng: XEM CHI TIẾT HÓA ĐƠN 
        public ActionResult ChiTietHoaDon(int id = 0)
        {
            using (phone_BETAEntities db = new phone_BETAEntities())
            {
                return View(db.CHITIET_HOADON.Where(x => x.ID_HD == id).ToList());
            }
        }
        // 3. Chức năng: HỦY HÓA ĐƠN
        public ActionResult HuyHoaDon()
        {
            using (phone_BETAEntities entities = new phone_BETAEntities())
            {
                return View(entities.Huy_HoaDon(("")));
            }
        }

        [HttpPost]
        public ActionResult HuyHoaDon(string id_hd)
        {
            using (phone_BETAEntities entities = new phone_BETAEntities())
            {
                return View(entities.Huy_HoaDon(id_hd));
            }
        }

        // 4. Chức năng: XÓA TÀI KHOẢN
        public ActionResult XoaTaiKhoan()
        {
            using (phone_BETAEntities entities = new phone_BETAEntities())
            {
                return View(entities.XoaTK_KHACHHANG(("")));
            }
        }

        [HttpPost]
        public ActionResult XoaTaiKhoan(string id_customer)
        {
            using (phone_BETAEntities entities = new phone_BETAEntities())
            {
                entities.XoaTK_KHACHHANG(id_customer);
                Session.Abandon();
                return RedirectToAction("Index", "Home");
            }
        }

        // 5. Chức năng: TÌM KIẾM HÓA ĐƠN THEO NGÀY
        public ActionResult TimKiemHoaDon()
        {
            phone_BETAEntities entities = new phone_BETAEntities();
            return View(entities.HoaDon_SearchHD_1("",""));
        }

        [HttpPost]
        public ActionResult TimKiemHoaDon(string DateBillFrom, string DateBillTo)
        {
            phone_BETAEntities entities = new phone_BETAEntities();
            return View(entities.HoaDon_SearchHD_1(DateBillFrom, DateBillTo));
        }
        // 6. Chức năng: XEM THÔNG TIN CÁ NHÂN
        public ActionResult ThongTinKH(int id)
        {
            using (phone_BETAEntities db = new phone_BETAEntities())
            {
                return View(db.KHACHHANGs.Where(x => x.ID_Customer == id).FirstOrDefault());
            }
        }

        // 7. Chức năng: THAY ĐỔI THÔNG TIN CÁ NHÂN
        public ActionResult SuaThongTin(int id)
        {
            using (phone_BETAEntities db = new phone_BETAEntities())
            {
                return View(db.KHACHHANGs.Where(x => x.ID_Customer == id).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult SuaThongTin(int id, KHACHHANG khachhang)
        {
            using (phone_BETAEntities db = new phone_BETAEntities())
            {
                db.Entry(khachhang).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("ThongTinKH", "KHACHHANG", new { id = Session["ID_Customer"] });
        }
    }
}