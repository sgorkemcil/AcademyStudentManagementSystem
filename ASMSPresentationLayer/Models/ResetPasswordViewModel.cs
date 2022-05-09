﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASMSPresentationLayer.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string UserID { get; set; }
        [Required]
        public string Code { get; set; }

        [Required(ErrorMessage = "Yeni şifre alanı zorunludur!")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Şifreniz minimum 8 maksimum 20 haneli olmalıdır!")]
        [Display(Name = "Yeni Şifre")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage ="Yeni Şifre Tekrar alanı zorunludur!")]
        [DataType(DataType.Password)]
        [Display(Name ="Yeni Şifre Tekrarı")]
        [Compare(nameof(NewPassword),ErrorMessage ="Şifreler uyuşmuyor!")]
        public string ConfirmNewPassWord { get; set; }
    }
}
