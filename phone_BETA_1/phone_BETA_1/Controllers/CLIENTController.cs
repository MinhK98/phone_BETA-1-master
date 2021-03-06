﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using phone_BETA_1.Models;

namespace phone_BETA_1.Controllers
{
    public class CLIENTController : Controller
    {
        // 1. Chức năng: ĐĂNG NHẬP
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authorize(phone_BETA_1.Models.CLIENT clientModel)
        {
            using (phone_BETAEntities db = new phone_BETAEntities())
            {
                var clientDetails = db.CLIENTS.Where(x => x.Log_Customer == clientModel.Log_Customer && x.Pass == clientModel.Pass).FirstOrDefault();
                if (clientDetails == null)
                {
                    clientModel.LoginErrorMessage = "Sai tên đăng nhập hoặc mật khẩu!";
                    return View("SignIn", clientModel);
                }
                else
                {
                    Session["ID_Customer"] = clientDetails.ID_Customer;
                    Session["Log_Customer"] = clientDetails.Log_Customer;
                    return RedirectToAction("HoaDon", "KHACHHANG", new { id = Session["ID_Customer"] });
                }
            }
        }

        public ActionResult LogOut()
        {
            int ID_Customer = (int)Session["ID_Customer"];
            Session.Abandon();
            return RedirectToAction("SignIn", "CLIENT");
        }
        // ------------------------------------------//

        // 2. Chức nằng: ĐĂNG KÝ
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(KHACHHANG kh, FormCollection frc)
        {
            using (phone_BETAEntities db = new phone_BETAEntities())
            {
                // 1. Tạo thông tin khách hàng:
                db.KHACHHANGs.Add(kh);
                db.SaveChanges();
                // 2. Tự động tạo tài khoản khách hàng:
                CLIENT cl = new CLIENT()
                {
                    ID_Customer = kh.ID_Customer,
                    Log_Customer = "Customer" + kh.ID_Customer,
                    Pass = "Customer" + kh.ID_Customer,
                    ConfirmPassWord = "Customer" +kh.ID_Customer,
                };
                db.CLIENTS.Add(cl);
                db.SaveChanges();
            }
            ModelState.Clear();
            return RedirectToAction("SignIn","CLIENT",new KHACHHANG());
        }

        // 3. Chức năng: CHỈNH SỬA THÔNG TIN TÀI KHOẢN
        public ActionResult Edit(int id)
        {
            using (phone_BETAEntities db = new phone_BETAEntities())
            {
                return View(db.CLIENTS.Where(x => x.ID_Customer == id).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, CLIENT client)
        {
            try
            {
                using (phone_BETAEntities db = new phone_BETAEntities())
                {
                    db.Entry(client).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("ThongTinKH", "KHACHHANG", new { id = Session["ID_Customer"] });
            }
            catch
            {
                return View();
            }
        }
    }
}