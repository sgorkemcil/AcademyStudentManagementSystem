using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASMSPresentationLayer.Areas.Management.Models
{
    public class RegisterAdminViewModel
    {
        
        [Display(Name = "isim")]
        [Required(ErrorMessage = "İsim Gereklidir!")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "İsminiz en az 2 en çok 50 karakter olmalıdır!")]
        public string Name { get; set; }
        [Display(Name = "Soy İsim")]
        [Required(ErrorMessage = "Soyisim Gereklidir!")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Soyisminiz en az 2 en çok 50 karakter olmalıdır!")]
        public string Surname { get; set; }
        [Display(Name = "Mail Adresi")]
        [Required(ErrorMessage = "Email zorunludur!")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Şifre alanı zorunludur!")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Şifreniz minimum 8 maksimum 20 haneli olmalıdır!")]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Yeni Şifre Tekrar alanı zorunludur!")]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre Tekrarı")]
        [Compare(nameof(Password), ErrorMessage = "Şifreler uyuşmuyor!")]
        public string ConfirmPassWord { get; set; }
    }
}
