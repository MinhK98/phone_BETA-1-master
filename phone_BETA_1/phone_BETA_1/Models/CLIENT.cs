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

    public partial class CLIENT
    {
        public int ID_Customer { get; set; }
        [Required(ErrorMessage = "Tên đăng nhập không được trống!")]
        public string Log_Customer { get; set; }
        [DisplayName("Mật khẩu:")]
        [DataType(DataType.Password)]
        public string Pass { get; set; }
        [DisplayName("Xác nhận mật khẩu:")]
        [DataType(DataType.Password)]
        [Compare("Pass")]
        public string ConfirmPassWord { get; set; }

        public string LoginErrorMessage { get; set; }

        public virtual KHACHHANG KHACHHANG { get; set; }
    }
}
