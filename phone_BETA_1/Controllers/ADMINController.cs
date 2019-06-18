using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using phone_BETA_1.Models;

namespace phone_BETA_1.Controllers
{
    public class ADMINController : Controller
    {
        // I. DBO.ADMIN:
        //// 1. Chức năng: ĐĂNG NHẬP
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authorize(phone_BETA_1.Models.ADMIN adminModel)
        {
            using (phone_BETAEntities1 db = new phone_BETAEntities1())
            {
                var adminDetails = db.ADMINS.Where(x => x.Log_Admin == adminModel.Log_Admin && x.Pass == adminModel.Pass).FirstOrDefault();
                if (adminDetails == null)
                {
                    adminModel.LoginErrorMessage = "Sai tên đăng nhập hoặc mật khẩu!";
                    return View("SignIn", adminModel);
                }
                else
                {
                    Session["ID_Admin"] = adminDetails.ID_Admin;
                    Session["Log_Admin"] = adminDetails.Log_Admin;
                    return RedirectToAction("KhachHang", "NHANVIEN");
                }
            }
        }
        //// ------------------------------------------////

        //// 2. Chức năng: ĐĂNG XUẤT
        public ActionResult LogOut()
        {
            int ID_Admin = (int)Session["ID_Admin"];
            Session.Abandon();
            return RedirectToAction("SignIn", "ADMIN");
        }
        //// ------------------------------------------////

        //// 3. Chức năng: ĐĂNG KÝ
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(NHANVIEN nv, FormCollection frc)
        {
            using (phone_BETAEntities1 db = new phone_BETAEntities1())
            {
                // 1. Tạo thông tin nhân viên:
                db.NHANVIENs.Add(nv);
                db.SaveChanges();
                // 2. Tự động tạo tài khoản nhân viên:
                ADMIN ad = new ADMIN()
                {
                    ID_Admin = nv.ID_Admin,
                    Log_Admin = "Staff" + nv.ID_Admin,
                    Pass = "Staff" + nv.ID_Admin,
                    ConfirmPassWord = "Staff" + nv.ID_Admin,
                };
                db.ADMINS.Add(ad);
                db.SaveChanges();
            }
            ModelState.Clear();
            return RedirectToAction("SignIn","ADMIN",new NHANVIEN());
        }
        //// ------------------------------------------////

        //// 3. Chức năng: CHỈNH SỬA THÔNG TIN TÀI KHOẢN
        public ActionResult Edit(int id)
        {
            using (phone_BETAEntities1 db = new phone_BETAEntities1())
            {
                return View(db.ADMINS.Where(x => x.ID_Admin == id).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, ADMIN admin)
        {
            try
            {
                using (phone_BETAEntities1 db = new phone_BETAEntities1())
                {
                    db.Entry(admin).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("ThongTinNV", "NHANVIEN", new { id = Session["ID_Admin"] });
            }
            catch
            {
                return View();
            }
        }
        //// ------------------------------------------////
        
        // II. DBO.SANPHAM:
    }
}