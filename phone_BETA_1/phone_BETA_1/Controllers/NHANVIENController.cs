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
        [HttpGet]
        public ActionResult ChiTietSanPham(int id)
        {
            using (phone_BETAEntities1 db = new phone_BETAEntities1())
            {
                phone_BETAEntities1 entities = new phone_BETAEntities1();
                db.SANPHAMs.Where(x => x.ID_SP == id).FirstOrDefault();              
                return View(entities.ChiTietSanPham(id.ToString()).FirstOrDefault());
            }
        }
        //// 3. Chức năng: THÊM MỚI SẢN PHẨM
        [HttpGet]
        public ActionResult ThemSanPham()
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
            using (phone_BETAEntities1 db = new phone_BETAEntities1())
            {
                SANPHAM sanpham = db.SANPHAMs.Where(x => x.ID_SP == id).FirstOrDefault();
                db.SANPHAMs.Remove(sanpham);
                db.SaveChanges();
            }
            return RedirectToAction("SanPham", "NHANVIEN");
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
            string fileName = Path.GetFileNameWithoutExtension(sanpham.ImageFile.FileName);
            string extension = Path.GetExtension(sanpham.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            sanpham.Photo = "~/Images/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
            sanpham.ImageFile.SaveAs(fileName);
            using (phone_BETAEntities1 db = new phone_BETAEntities1())
            {
                db.Entry(sanpham).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("SanPham", "NHANVIEN");
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
            return View(entities.SearchHD_2("", "",""));
        }

        [HttpPost]
        public ActionResult TimKiemHoaDon(string NameCustomer, string DateBillFrom, string DateBillTo)
        {
            phone_BETAEntities1 entities = new phone_BETAEntities1();
            return View(entities.SearchHD_2(NameCustomer, DateBillFrom, DateBillTo));
        }

        //// 5. Chức năng: TÌM KIẾM HÓA ĐƠN THEO NGÀY
        public ActionResult TimKiemHoaDon_Ngay()
        {
            phone_BETAEntities1 entities = new phone_BETAEntities1();
            return View(entities.SearchHD_0("",""));
        }

        [HttpPost]
        public ActionResult TimKiemHoaDon_Ngay(string DateBillFrom, string DateBillTo)
        {
            phone_BETAEntities1 entities = new phone_BETAEntities1();
            return View(entities.SearchHD_0(DateBillFrom, DateBillTo));
        }
        //// 6. Chức năng: TÌM KIẾM HÓA ĐƠN THEO TÊN KHÁCH HÀNG
        public ActionResult TimKiemHoaDon_Name()
        {
            phone_BETAEntities1 entities = new phone_BETAEntities1();
            return View(entities.SearchHD_3(""));
        }

        [HttpPost]
        public ActionResult TimKiemHoaDon_Name(string NameCustomer)
        {
            phone_BETAEntities1 entities = new phone_BETAEntities1();
            return View(entities.SearchHD_3(NameCustomer));
        }
        //// 7. Chức năng: TÌM KIẾM HÓA ĐƠN THEO TÌNH TRẠNG
        public ActionResult TimKiemHoaDon_TinhTrang()
        {
            phone_BETAEntities1 entities = new phone_BETAEntities1();
            return View(entities.Search_HD_1(""));
        }

        [HttpPost]
        public ActionResult TimKiemHoaDon_TinhTrang(string TinhTrang)
        {
            phone_BETAEntities1 entities = new phone_BETAEntities1();
            return View(entities.Search_HD_1(TinhTrang));
        }
        //-----------------------------//

        // IV. DBO.SANPHAM:

        //// 1. Chức năng: TÌM KIẾM THEO LOẠI HÀNG
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
        //// 2. Chức năng: TÌM KIẾM THEO NHÓM HÀNG
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
        //// 3. Chức năng: TÌM KIẾM THEO HÃNG SẢN XUẤT
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
        //// 4. Chức năng: TÌM KIẾM THEO CẢ 3
        public ActionResult TimKiemSanPham_Ca3()
        {
            phone_BETAEntities1 entities = new phone_BETAEntities1();
            return View(entities.Search_SP_3("","",""));
        }

        [HttpPost]
        public ActionResult TimKiemSanPham_Ca3(string NH, string HSX, string LH)
        {
            phone_BETAEntities1 entities = new phone_BETAEntities1();
            return View(entities.Search_SP_3(NH, HSX, LH));
        }
        //// 5. Chức năng: TÌM KIẾM THEO TÌNH TRẠNG
        public ActionResult TimKiemSanPham_TinhTrang()
        {
            phone_BETAEntities1 entities = new phone_BETAEntities1();
            return View(entities.Search_SP_TinhTrang(""));
        }

        [HttpPost]
        public ActionResult TimKiemSanPham_TinhTrang(string TinhTrang)
        {
            phone_BETAEntities1 entities = new phone_BETAEntities1();
            return View(entities.Search_SP_TinhTrang(TinhTrang));
        }
        //// 6. Chức năng: TÌM KIẾM THEO TÊN SẢN PHẨM
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
        //-----------------------------//

        // V. DBO.LOAIHANG:

        //// 1. Chức năng: XEM DANH SÁCH LOẠI HÀNG
        public ActionResult LoaiHang()
        {
            using (phone_BETAEntities1 db = new phone_BETAEntities1())
            {
                return View(db.LOAIHANGs.ToList());
            }
        }

        //// 2. Chức năng: THÊM LOẠI HÀNG
        [HttpGet]
        public ActionResult ThemLoaiHang(int id = 0)
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemLoaiHang(LOAIHANG loaihang, FormCollection frc)
        {
            using (phone_BETAEntities1 db = new phone_BETAEntities1())
            {
                db.LOAIHANGs.Add(loaihang);
                db.SaveChanges();
            }
            ModelState.Clear();
            return View();
        }
        //// 3. Chức năng: SỬA TÊN LOẠI HÀNG
        public ActionResult SuaLoaiHang(int id)
        {
            using (phone_BETAEntities1 db = new phone_BETAEntities1())
            {
                return View(db.LOAIHANGs.Where(x => x.ID_LH == id).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult SuaLoaiHang(int id, LOAIHANG loaihang)
        {
            using (phone_BETAEntities1 db = new phone_BETAEntities1())
            {
                db.Entry(loaihang).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("LoaiHang", "NHANVIEN");
        }
        //-----------------------------//

        // VI. DBO.NHOMHANG:

        //// 1. Chức năng: XEM DANH SÁCH NHÓM HÀNG
        public ActionResult NhomHang()
        {
            using (phone_BETAEntities1 db = new phone_BETAEntities1())
            {
                return View(db.NHOMHANGs.ToList());
            }
        }

        //// 2. Chức năng: THÊM NHÓM HÀNG
        [HttpGet]
        public ActionResult ThemNhomHang(int id = 0)
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemNhomHang(NHOMHANG nhomhang, FormCollection frc)
        {
            using (phone_BETAEntities1 db = new phone_BETAEntities1())
            {
                db.NHOMHANGs.Add(nhomhang);
                db.SaveChanges();
            }
            ModelState.Clear();
            return View();
        }
        //// 3. Chức năng: SỬA TÊN NHÓM HÀNG
        public ActionResult SuaNhomHang(int id)
        {
            using (phone_BETAEntities1 db = new phone_BETAEntities1())
            {
                return View(db.NHOMHANGs.Where(x => x.ID_NH == id).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult SuaNhomHang(int id, NHOMHANG nhomhang)
        {
            using (phone_BETAEntities1 db = new phone_BETAEntities1())
            {
                db.Entry(nhomhang).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("NhomHang", "NHANVIEN");
        }
        //-----------------------------//

        // VII. DBO.HANGSX:

        //// 1. Chức năng: XEM DANH SÁCH HÃNG SẢN XUẤT
        public ActionResult HangSX()
        {
            using (phone_BETAEntities1 db = new phone_BETAEntities1())
            {
                return View(db.HANG_SX.ToList());
            }
        }

        //// 2. Chức năng: THÊM HÃNG SẢN XUẤT
        [HttpGet]
        public ActionResult ThemHangSX(int id = 0)
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemHangSX(HANG_SX hangsx, FormCollection frc)
        {
            using (phone_BETAEntities1 db = new phone_BETAEntities1())
            {
                db.HANG_SX.Add(hangsx);
                db.SaveChanges();
            }
            ModelState.Clear();
            return View();
        }
        //// 3. Chức năng: SỬA TÊN HÃNG SẢN XUẤT
        public ActionResult SuaHangSX(int id)
        {
            using (phone_BETAEntities1 db = new phone_BETAEntities1())
            {
                return View(db.HANG_SX.Where(x => x.ID_HSX == id).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult SuaHangSX(int id, HANG_SX hangsx)
        {
            using (phone_BETAEntities1 db = new phone_BETAEntities1())
            {
                db.Entry(hangsx).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("HangSX", "NHANVIEN");
        }
        //-----------------------------//

        // VII. DBO.ADMIN:

        //// 1. Chức năng: Xóa tài khoản nhân viên
        public ActionResult XoaTaiKhoan()
        {
            using (phone_BETAEntities1 entities = new phone_BETAEntities1())
            {
                return View(entities.XoaTK_NHANVIEN(("")));
            }
        }

        [HttpPost]
        public ActionResult XoaTaiKhoan(string id_admin)
        {
            using (phone_BETAEntities1 entities = new phone_BETAEntities1())
            {
                entities.XoaTK_NHANVIEN(id_admin);
                Session.Abandon();
                return RedirectToAction("KHACHHANG", "NHANVIEN");
            }
        }

        // 2. Chức năng: XEM THÔNG TIN CÁ NHÂN
        public ActionResult ThongTinNV(int id)
        {
            using (phone_BETAEntities1 db = new phone_BETAEntities1())
            {
                return View(db.NHANVIENs.Where(x => x.ID_Admin == id).FirstOrDefault());
            }
        }

        // 3. Chức năng: THAY ĐỔI THÔNG TIN CÁ NHÂN
        public ActionResult SuaThongTin(int id)
        {
            using (phone_BETAEntities1 db = new phone_BETAEntities1())
            {
                return View(db.NHANVIENs.Where(x => x.ID_Admin == id).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult SuaThongTin(int id, NHANVIEN nhanvien)
        {
            using (phone_BETAEntities1 db = new phone_BETAEntities1())
            {
                db.Entry(nhanvien).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("ThongTinNV", "NHANVIEN", new { id = Session["ID_Admin"] });
        }
    }
}