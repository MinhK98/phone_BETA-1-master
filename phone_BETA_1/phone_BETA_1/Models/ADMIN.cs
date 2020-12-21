﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace phone_BETA_1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class ADMIN
    {
        [DisplayName("Mã nhân viên:")]
        public int ID_Admin { get; set; }
        [DisplayName("Tên đăng nhập:")]
        [Required(ErrorMessage = "Tên đăng nhập không được trống!")]
        public string Log_Admin { get; set; }
        [DisplayName("Mật khẩu:")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Mật khẩu không được trống!")]
        public string Pass { get; set; }
        [DisplayName("Xác nhận mật khẩu:")]
        [DataType(DataType.Password)]
        [Compare("Pass")]
        public string ConfirmPassWord { get; set; }
        public string LoginErrorMessage { get; set; }
        public string Authorize { get; set; }

        public virtual NHANVIEN NHANVIEN { get; set; }
    }
}
