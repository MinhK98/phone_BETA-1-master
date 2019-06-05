﻿-- QUẢN LÝ SÁCH:
select SinhVien.MaSV AS [Mã Sinh Viên],TenSV AS [Tên Sinh Viên],SinhVien.MaLop AS [Mã Lớp],MuonSach.MaSach AS [Mã Sách],NgayMuon AS [Ngày Mượn] 
from SinhVien,MuonSach 
where SinhVien.MaSV=MuonSach.MaSV and GETDATE()>=NgayMuon

select top 10 with ties Sach.MaSach AS [Mã Sách], SL1 AS [Số Lượng Mượn] 
from Sach ,(select sum(SoLuong) SL1, MaSach from MuonSach group by MaSach) s1 
where Sach.MaSach = s1.MaSach 
order by SL1 DESC

select top 1 with ties Sach.MaSach AS [Mã Sách],Sach.MaTacGia AS [Mã Tác Giả],TenTacGia AS [Tên Tác Giả],SL1 AS [Số Lượng Mượn] 
from Sach,TacGia,(select sum(SoLuong) SL1, MaSach from MuonSach group by MaSach) s1 
where Sach.MaSach = s1.MaSach and TacGia.MaTacGia=Sach.MaTacGia 
order by SL1 DESC

-- QUẢN LÝ GIÁO VIÊN:
---> 1. Định mức nghiên cứu với chức danh chuyên môn nghiệp vụ và học hàm: 
create proc DinhMuc_NghienCuu (@maCDCM char(10), @maHocHam char(10))
as
begin
	select chucDanh as 'Chức danh', tenHocHam as 'Tên hoc hàm', dinhMucThoiGian as 'Định mức thời gian', dinhMucGioChuan as 'Định mức giờ chuẩn' 
	from DINH_MUC_NGHIEN_CUU as DMNC, CHUC_DANH_CHUYEN_MON_NGHIEP_VU as CHCMNV, HOC_HAM as HH
	where DMNC.maCDCM = CHCMNV.maCDCM and DMNC.maHocHam = HH.maHocHam and CHCMNV.maCDCM = @maCDCM and HH.maHocHam = @maHocHam
end
go

DinhMuc_NghienCuu 'CD06', 'HH01'

---> 2. Xóa giáo viên nghiên cứu --> xóa luôn cả sản phẩm và giải thưởng:
create proc Xoa_GV_Nghien_cuu (@maGV char(10))
as
begin
	delete from SAN_PHAM_KHCN where maGV = @maGV
	delete from GIAI_THUONG_KHCN where maGV = @maGV
	delete from GIAO_VIEN_NGHIEN_CUU where maGV = @maGV
end
go

Xoa_GV_Nghien_cuu 'GV02'

---> Xóa Nghiên cứu --> xóa luôn giáo viên có nghiên cứu đó:
create proc Xoa_GV_DeTai_NghienCuu (@maDeTai char(10))
as
begin
	delete from GIAO_VIEN_NGHIEN_CUU where maDeTai = @maDeTai
	delete from NGHIEN_CUU where maDeTai = @maDeTai
end
go


---> Liệt kê mã giáo viên, tên sản phẩm, ghi chú và hình thức theo mã giáo viên:
create proc Liet_ke_San_Pham (@maGV char(10))
as
begin
	select GV.maGV as 'Mã giáo viên', tenSP as 'Tên sản phẩm', ghiChu as 'Ghi chú'
	from GIAO_VIEN_NGHIEN_CUU as GV, SAN_PHAM_KHCN as SP
	where GV.maGV = SP.maGV and GV.maGV = @maGV
	group by GV.maGV, tenSP, ghiChu
end
go

Liet_ke_San_Pham 'GV01'

--> Liệt kê mã giáo viên, tên giải thưởng, hình thức theo mã giáo viên:
create proc Liet_ke_Giai_Thuong (@maGV char(10))
as
begin
	select GV.maGV as 'Mã giáo viên', hinhThuc as 'Hình thức'
	from GIAO_VIEN_NGHIEN_CUU as GV, GIAI_THUONG_KHCN as GT
	where GV.maGV = GT.maGV and GV.maGV = @maGV
end
go

Liet_ke_Giai_Thuong 'GV05'

--> Đếm số sản phẩm (hoặc giải thưởng) của giáo viên đó.
create proc Liet_ke_GT_va_SP (@maGV char(10))
as
begin
	select GV.maGV as 'Mã giáo viên' , count(SP.maSP) as 'Số sản phẩm', count (GT.maGT) as 'Số giải thưởng'
	from GIAO_VIEN_NGHIEN_CUU as GV, GIAI_THUONG_KHCN as GT, SAN_PHAM_KHCN as SP
	where GV.maGV = GT.maGV and GV.maGV = SP.maGV and GV.maGV = @maGV
	group by GV.maGV
end
go

Liet_ke_GT_va_SP 'GV04'

--> Liệt kê sản phẩm và sản phẩm của các giáo viên:
select GV.maGV as 'Mã giáo viên' ,GT.maGT as 'Mã giải thưởng', GT.hinhThuc as 'Hình thức', SP.tenSP as 'Tên sản phẩm'
from GIAO_VIEN_NGHIEN_CUU as GV, GIAI_THUONG_KHCN as GT, SAN_PHAM_KHCN as SP
where GV.maGV = GT.maGV and GV.maGV = SP.maGV 
group by GV.maGV, GT.maGT,GT.hinhThuc, SP.tenSP

--> Liệt kê sản phẩm và giải thưởng của các giáo viên: (STORE PROCEDURE)
create proc GT_SP_Giao_vien (@maGV char(10))
as
begin
	select GV.maGV as 'Mã giáo viên' ,GT.maGT as 'Mã giải thưởng', GT.hinhThuc as 'Hình thức', SP.tenSP as 'Tên sản phẩm'
	from GIAO_VIEN_NGHIEN_CUU as GV, GIAI_THUONG_KHCN as GT, SAN_PHAM_KHCN as SP
	where GV.maGV = GT.maGV and GV.maGV = SP.maGV and GV.maGV = @maGV
	group by GV.maGV, GT.maGT,GT.hinhThuc, SP.tenSP
end
go

GT_SP_Giao_vien 'GV05'

--> Tìm ra giáo viên nghiên cứu có số giải thưởng nhiều nhất:
select top 1 with ties GV.maGV as 'Mã giáo viên' , SL1 as 'Số giải thưởng'
from GIAO_VIEN_NGHIEN_CUU as GV, (select count(maGT) SL1, maGV from GIAI_THUONG_KHCN group by maGV) s1
where GV.maGV = s1.maGV 
order by SL1 DESC

--> Tìm ra giáo viên nghiên cứu có số sản phẩm nhiều nhất:
select top 1 with ties GV.maGV as 'Mã giáo viên', SL1 as 'Số sản phẩm'
from GIAO_VIEN_NGHIEN_CUU as GV ,(select count(maSP) SL1, maGV from SAN_PHAM_KHCN group by maGV) s1
where GV.maGV = s1.maGV
order by SL1 DESC

--> Tìm ra giáo viên nghiên cứu có số sản phẩm và giải thưởng nhiều nhất:
select top 1 with ties GV.maGV as 'Mã giáo viên' , SL1 as 'Số sản phẩm', SL2 as 'Số giải thưởng'
from GIAO_VIEN_NGHIEN_CUU as GV, (select count(maSP) SL1, maGV from SAN_PHAM_KHCN group by maGV) s1, (select count(maGT) SL2, maGV from GIAI_THUONG_KHCN group by maGV) s2
where GV.maGV = s1.maGV
order by SL1 DESC, SL2 DESC

select GVNC.maGV as 'Mã giáo viên', tenDeTai as 'Tên đề tài', chucDanh as 'Chức danh', thoiDiemNhan as 'Thời điểm nhận'
from CHI_TIET_CDCMNV as CT, GIAO_VIEN_NGHIEN_CUU as GVNC, NGHIEN_CUU as NC, CHUC_DANH_CHUYEN_MON_NGHIEP_VU as CD
where GVNC.maGV = CT.maGV and GVNC.maDeTai = NC.maDeTai and CT.maCDCM = CD.maCDCM and GVNC.maGV = 'GV05'


select maDeTai as 'Mã đề tài', tenDeTai as 'Tên đề tài', donViTinh as 'Đơn vị tính', gioChuan as 'Giờ chuẩn' from NGHIEN_CUU

select maLoaiBaiBao as 'Mã loại bài báo', tenLoaiBaiBao as 'Tên loại bài báo', donViTinh as 'Đơn vị tính', gioChuan as 'Giờ chuẩn' from LOAI_BAI_BAO

select maBaiBao as 'Mã bài báo', tenBaiBao as 'Tên bài báo', ngayCongBo as 'Ngày công bố', so as 'Số', tapChiCongBo as 'Tạp chí công bố', maLoaiBaiBao as 'Mã loại bài ' from BAI_BAO

select maLoaiSach as 'Mã loại sách', tenLoaiSach as 'Tên loại sách', donViTinh as 'Đơn vị tính', gioChuan as 'Giờ chuẩn' from LOAI_SACH

select maSach as 'Mã sách', tenSach as 'Tên sách', namXuatBan as 'Năm xuất bản', noiXuatBan as 'Nơi xuất bản', maLoaiSach as 'Mã loại sách'  from SACH

ALTER AUTHORIZATION 
ON DATABASE:: [phone_BETA]
TO [DESKTOP-DDPNBEB\minhl];

-- QUẢN LÝ BÁN ĐIỆN THOẠI:
--- Tìm kiếm hóa đơn của khách hàng theo ngày và tên khách hàng: (PHÍA ADMIN)
CREATE PROCEDURE HoaDon_SearchHD (@Name_Customer NVARCHAR(50), @DateCreate nvarchar(50))
AS
BEGIN
	  DECLARE @query nvarchar(50)
      SET NOCOUNT ON;
	  SELECT @query = dbo.non_unicode_convert(@Name_Customer)
      SELECT *
      FROM HOADON
      WHERE dbo.non_unicode_convert(Name_Customer) LIKE '%' + @query + '%' and DateCreate = @DateCreate
END

HoaDon_SearchHD 'Vuong', '2019-06-01'

--- Tìm kiếm hóa đơn theo ngày: (PHÍA CLIENT)
CREATE PROCEDURE HoaDon_SearchHD_2 (@DateCreate nvarchar(50))
AS
BEGIN
      SET NOCOUNT ON;
      SELECT *
      FROM HOADON
      WHERE DateCreate = @DateCreate
END

HoaDon_SearchHD_2 '2019-06-01'

--- Tự động xóa thông tin trong bảng CLIENT khi xóa thông tin KHACHHANG:
create proc XoaTK_KHACHHANG (@ID_Customer nvarchar(3))
as 
begin 
	DECLARE @ID_HD AS NVARCHAR(3);
	DELETE from CLIENTS where ID_Customer=@ID_Customer;
	SELECT @ID_HD = ID_HD from HOADON where ID_Customer = @ID_Customer
	DELETE from CHITIET_HOADON where CHITIET_HOADON.ID_HD = @ID_HD
	DELETE from HOADON where ID_HD = @ID_HD
	DELETE from KHACHHANG where ID_Customer=@ID_Customer;
end 
go 

XoaTK_KHACHHANG ''

--- Xóa sản phẩm đi thì xóa luôn hóa đơn và chi tiết hóa đơn của khách hàng:
create proc Xoa_SanPham (@ID_SP nvarchar(3))
as 
begin 
	DECLARE @ID_HD AS NVARCHAR(3);
	SELECT @ID_HD = ID_HD from CHITIET_HOADON where ID_SP = @ID_SP
	DELETE from CHITIET_HOADON where ID_HD = @ID_HD
	DELETE from HOADON where HOADON.ID_HD = @ID_HD
	DELETE from SANPHAM where ID_SP = @ID_SP
end 
go 

Xoa_SanPham ''

--- Trường hợp khách hàng muốn hủy hóa đơn:
create proc Huy_HoaDon (@ID_HD nvarchar(3))
as 
begin 
	DELETE from CHITIET_HOADON where ID_HD = @ID_HD;
	DELETE from HOADON where ID_HD = @ID_HD;
end 
go 

Huy_HoaDon ''

--- Hàm chuyển đổi Tiếng Việt có dấu thành không dấu:
CREATE FUNCTION [dbo].[non_unicode_convert](@inputVar NVARCHAR(MAX) )
RETURNS NVARCHAR(MAX)
AS
BEGIN    
    IF (@inputVar IS NULL OR @inputVar = '')  RETURN ''
   
    DECLARE @RT NVARCHAR(MAX)
    DECLARE @SIGN_CHARS NCHAR(256)
    DECLARE @UNSIGN_CHARS NCHAR (256)
 
    SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệếìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵýĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ' + NCHAR(272) + NCHAR(208)
    SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeeeiiiiiooooooooooooooouuuuuuuuuuyyyyyAADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD'
 
    DECLARE @COUNTER int
    DECLARE @COUNTER1 int
   
    SET @COUNTER = 1
    WHILE (@COUNTER <= LEN(@inputVar))
    BEGIN  
        SET @COUNTER1 = 1
        WHILE (@COUNTER1 <= LEN(@SIGN_CHARS) + 1)
        BEGIN
            IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) = UNICODE(SUBSTRING(@inputVar,@COUNTER ,1))
            BEGIN          
                IF @COUNTER = 1
                    SET @inputVar = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@inputVar, @COUNTER+1,LEN(@inputVar)-1)      
                ELSE
                    SET @inputVar = SUBSTRING(@inputVar, 1, @COUNTER-1) +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@inputVar, @COUNTER+1,LEN(@inputVar)- @COUNTER)
                BREAK
            END
            SET @COUNTER1 = @COUNTER1 +1
        END
        SET @COUNTER = @COUNTER +1
    END
    -- SET @inputVar = replace(@inputVar,' ','-')
    RETURN @inputVar
END

--- Tìm kiếm tên khách hàng (STORE PROCEDURE):
CREATE PROCEDURE KhachHang_SearchKhachHang
      @Name_Customer NVARCHAR(50)
AS
BEGIN
	  DECLARE @query nvarchar(50)
      SET NOCOUNT ON;
	  SELECT @query = dbo.non_unicode_convert(@Name_Customer)
      SELECT *
      FROM KHACHHANG
      WHERE dbo.non_unicode_convert(Name_Customer) LIKE '%' + @query + '%'
END

KhachHang_SearchKhachHang ''

--- Tìm kiếm theo tên sản phẩm:
CREATE PROCEDURE SanPham_SearchTenSP
      @Name_SP NVARCHAR(50)
AS
BEGIN
	  DECLARE @query nvarchar(50)
      SET NOCOUNT ON;
	  SELECT @query = dbo.non_unicode_convert(@Name_SP)
      SELECT *
      FROM SANPHAM
      WHERE dbo.non_unicode_convert(Name_SP) LIKE '%' + @query + '%'
END

SanPham_SearchTenSP 'Logi'

--- Tìm kiếm theo hãng sản xuất:
CREATE PROCEDURE SanPham_SearchHSX
      @ID_HSX nvarchar(3)
AS
BEGIN
	  DECLARE @query nvarchar(50)
      SET NOCOUNT ON;
      SELECT *
      FROM SANPHAM
      WHERE ID_HSX = @ID_HSX
END

SanPham_SearchHSX '3'
--- Tìm kiếm theo loại hàng:
CREATE PROCEDURE SanPham_SearchLH
      @ID_LH nvarchar(3)
AS
BEGIN
      SET NOCOUNT ON;
      SELECT *
      FROM SANPHAM
      WHERE ID_LH = @ID_LH
END

SanPham_SearchLH ''
--- Tìm kiếm theo nhóm hàng:
CREATE PROCEDURE SanPham_SearchNH
      @ID_NH nvarchar(3)
AS
BEGIN
      SET NOCOUNT ON;
      SELECT *
      FROM SANPHAM
      WHERE ID_NH = @ID_NH
END

SanPham_SearchNH ''

--- Tìm kiếm theo cả 3: 
CREATE PROCEDURE SanPham_Search_3
      @ID_NH nvarchar(3), @ID_HSX nvarchar(3), @ID_LH nvarchar(3)
AS
BEGIN
      SET NOCOUNT ON;
      SELECT *
      FROM SANPHAM
      WHERE ID_NH = @ID_NH and ID_LH = @ID_LH and ID_HSX = @ID_HSX
END

SanPham_Search '','',''

--- Tìm kiếm theo tình trạng:
CREATE PROCEDURE SanPham_SearchTinhTrang
      @TinhTrang nvarchar(30)
AS
BEGIN
      DECLARE @query nvarchar(50)
      SET NOCOUNT ON;
	  SELECT @query = dbo.non_unicode_convert(@TinhTrang)
      SELECT *
      FROM SANPHAM
      WHERE dbo.non_unicode_convert(TinhTrang) LIKE '%' + @query + '%'
END

SanPham_SearchTinhTrang 'Co'



