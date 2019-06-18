using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using phone_BETA_1.Models;

namespace phone_BETA_1.Controllers
{
    public class CartController : Controller
    {
        private phone_BETAEntities1 db = new phone_BETAEntities1();
        // GET: CART
        public ActionResult Index()
        {
            return View();
        }

        private int isExisting(int id)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].Sanpham.ID_SP == id)
                    return i;
            return -1;
        }

        public ActionResult Delete(int id)
        {
            int index = isExisting(id);
            List<Item> cart = (List<Item>)Session["cart"];
            cart.RemoveAt(index);
            Session["cart"] = cart;
            return View("OrderNow");
        }

        public ActionResult OrderNow(int id)
        {
            if (Session["cart"] == null)
            {
                List<Item> cart = new List<Item>();
                cart.Add(new Item(db.SANPHAMs.Find(id), 1));
                Session["cart"] = cart;
            }
            else
            {
                List<Item> cart = (List<Item>)Session["cart"];
                int index = isExisting(id);
                if (index == -1)
                    cart.Add(new Item(db.SANPHAMs.Find(id), 1));
                else
                    cart[index].Quantity++;
                Session["cart"] = cart;
            }
            return View("OrderNow");
        }

        public ActionResult ProcessOrder(FormCollection frc)
        {
            using (phone_BETAEntities1 db = new phone_BETAEntities1())
            {
                List<Item> lsItem = (List<Item>)Session["cart"];
                // 1. Lưu vào bảng hóa đơn:
                HOADON hoadon = new HOADON()
                {
                    Name_Customer = frc["cusName"],
                    SDT = frc["cusPhone"],
                    Email = frc["cusEmail"],
                    DiaChi = frc["cusAddress"],
                    DateCreate = DateTime.Now,
                    ID_Customer = Convert.ToInt32(frc["cusID"]), 
                    TongTien = frc["totalPrice"],
                    TinhTrang = "Đang xử lý"
                };
                db.HOADONs.Add(hoadon);
                db.SaveChanges();
                //  2. Lưu vào bảng chi tiết hóa đơn:
                foreach (Item item in lsItem)
                {
                    CHITIET_HOADON cthd = new CHITIET_HOADON()
                    {
                        ID_HD = hoadon.ID_HD,
                        ID_SP = item.Sanpham.ID_SP,
                        SoLuong = item.Quantity,
                        Price_CTHD = item.Sanpham.Price_SP,
                        Name_SP = item.Sanpham.Name_SP,
                        Photo = item.Sanpham.Photo
                    };
                    db.CHITIET_HOADON.Add(cthd);
                    db.SaveChanges();
                }
                // 3. Xóa session giỏ hàng:
                Session.Remove("cart");
                return View("Index","Home");
            }
        }
    }
}