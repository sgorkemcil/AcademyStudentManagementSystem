using ASMSBusinessLayer.EmailService;
using ASMSEntityLayer.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASMSBusinessLayer.ContractsBLL;
using ASMSEntityLayer.Enums;
using ASMSEntityLayer.ViewModels;
using ASMSEntityLayer.ResultModels;
using ASMSBusinessLayer.ViewModels;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;
using ASMSPresentationLayer.Models;

namespace ASMSPresentationLayer.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly IStudentBusinessEngine _studentBusinessEngine;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager,
            IEmailSender emailSender, IStudentBusinessEngine studentBusinessEngine)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _studentBusinessEngine = studentBusinessEngine;
        }

        //[HttpGet]
        //public IActionResult Register()
        //{
        //    return View();
        //}

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    //return View(model);
                    TempData["RegisterFailedMessage"] = "Veri girişlerini istenildiği gibi yapmadınız!Tekrar deneyiniz!";
                    return RedirectToAction("Index", "Home");
                }

                //Aynı emailden tekrar kayıt olunmasın 
                var checkUserForEmail = await _userManager.FindByIdAsync(model.Email);
                if (checkUserForEmail != null)
                {
                    //ModelState.AddModelError("", "Bu email ile zaten sisteme kayıt yapılmıştır!");
                    //return View(model);
                    TempData["RegisterFailedMessage"] = "Bu email ile zaten sisteme kayıt yapılmıştır!";
                    return RedirectToAction("Index", "Home");
                }
                //user'ı oluşturalım

                AppUser newUser = new AppUser()
                {
                    Email = model.Email,
                    Name = model.Name,
                    Surname = model.Surname,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    BirthDate = model.BirthDate.HasValue ? model.BirthDate.Value : null,
                    Gender = model.Gender,
                    EmailConfirmed = true,
                    UserName = model.Email,
                    TCNumber=model.TCNumber
                };

                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)//Eğer result başarılı olduysa ekleme yapsın (eklendi)
                {
                    //rol ataması
                    var roleResult = await _userManager.AddToRoleAsync(newUser, ASMSRoles.Student.ToString());//Buraya Geri Döneceğiz
                    if (roleResult.Succeeded == false)
                    {
                        //Admine gizliden bir email gönder eklesin rolü
                    }

                    //Student eklensin
                    StudentVM newStudent = new StudentVM()
                    {
                        UserId = newUser.Id,
                        TCNumber = model.TCNumber
                    };
                    IResult resultStudent = _studentBusinessEngine.Add(newStudent);
                    if (resultStudent.IsSuccess == false)
                    {
                        //Admine gizliden bir email gönder eklesin öğrenciyi
                    }
                    //Email gönderilsin
                    var emailToStudent = new EmailMessage()
                    {
                        Subject = "ASMS SİSTEMİNE HOŞ GELDİNİZ!" + newUser.Name + " " + newUser.Surname,
                        Body = "Merhaba Sisteme Kaydınız Gerçekleşmiştir...",
                        Contacts = new string[] { model.Email }
                    };
                    await _emailSender.SendMessage(emailToStudent);
                    TempData["RegisterSucccessMessage"] = "Sisteme kaydınız başarıyla gerçekleşti!";
                    return RedirectToAction("Index", "Home", new { email = model.Email });
                }
                else
                {
                    TempData["RegisterFailedMessage"] = "Sisteme kaydınız başarıyla gerçekleşti!";
                    return RedirectToAction("Index", "Home");
                    //ModelState.AddModelError("", "Beklenmedik bir sorun oldu. Üye kaydınız başarısız tekrar deneyiniz! ");
                    //return View(model);
                }
            }
            catch (Exception)
            {

                //loglanacak
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public IActionResult Login(string email)
        {
            LoginViewModel model = new LoginViewModel()
            {
                Email = email
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var user = await _userManager.FindByNameAsync(model.Email);
                //var user=_userManager.FindByEmailAsync(model.Email);
                if(user==null)
                {
                    ModelState.AddModelError("", "Epostanız yada şifreniz hatalıdır!Tekrar deneyiniz!");
                    return View();
                }
                //TODO:son parametre bool lockoutOnFailure ile ilgili örnek yapalım
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                //TODO: son parametre bool lockoutOnFailure ile ilgili

                //if (result.IsLockedOut)
                //{
                //   DateTimeOffset d=user.LockoutEnd.Value
                //}
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Epostanız yada şifreniz hatalıdır!Tekrar deneyiniz!");
                    return View();
                }
                //artık hoşgeldiniz.
                if (_userManager.IsInRoleAsync(user,ASMSRoles.Student.ToString()).Result)
                {
                    return RedirectToAction("Dashboard", "Home");
                }
                if (_userManager.IsInRoleAsync(user, ASMSRoles.Coordinator.ToString()).Result)
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                if (_userManager.IsInRoleAsync(user, ASMSRoles.StudentAdministration.ToString()).Result)
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                return RedirectToAction("Dashboard", "Home");

            }
            catch (Exception)
            {

                ModelState.AddModelError("", "Beklenmedik bir hata oluştu! Tekrar deneyiniz");
                //ex loglancak
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(string email)
        {
            try
            {
                var user=await _userManager.FindByEmailAsync (email);
                if (user==null)
                {
                    ViewBag.ResetPasswordSuccessMessage = "Şifre yenileme talebiniz alını!Epostanızı kontrol ediniz!";
                }
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var codeEncode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callBackUrl =
                    Url.Action("ConfirmResetPassword", "Account", new { userId = user.Id, code = codeEncode },protocol: Request.Scheme);
               var emailMessage = new EmailMessage()
                {
                    Contacts = new string[] { user.Email },
                    Subject="ASMS-Yeni Şifre Talebi",
                    Body=$"Merhaba{user.Name}{user.Surname},"+
                    $"<br/>Yeni parola belirlemek için"+
                    $"<a href='{HtmlEncoder.Default.Encode(callBackUrl)}'>buraya </a>tıklayınız..."
                };
                await _emailSender.SendMessage(emailMessage);
                ViewBag.ResetPassWordSuccessMessage =
                    "Şifre yenileme talebiniz alındı!Epostanızı kontrol ediniz!";
                return View();
            }
            catch (Exception ex)
            {
                //ex loglansın
                ViewBag.ResetPasswordFailMessage = "Beklenmedik bir hata oluştu!Tekrar deneyiniz!";
                return View();
            }
        }
        [HttpGet]
        public IActionResult ConfirmResetPassword(string userId,string code)
        {
            if(string.IsNullOrEmpty(userId)||string.IsNullOrEmpty(code))
            {
                ViewBag.ConfirmResetPasswordFailureMessage = "Beklenmedik bir hata oluştu!";
                return View();
            }
            ResetPasswordViewModel model = new ResetPasswordViewModel()
            {
                UserID = userId,
                Code = code
            };
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> ConfirmResetPassword(ResetPasswordViewModel model)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return View(model);
                }
                var user = await _userManager.FindByIdAsync(model.UserID);
                if (User==null)
                {
                    ModelState.AddModelError("", "Kullanıcı Bulunamadı!");
                    //log mesajı yerleştir.
                    //throw new Exception();
                }
                var tokenDecoded = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));
                var result = await _userManager.ResetPasswordAsync(user, tokenDecoded, model.NewPassword);
                if(result.Succeeded)
                {
                    TempData["ConfirmResetPasswordSuccess"] = "Şifreniz başarıyla güncellenmiştir!";
                    return RedirectToAction("Login", "Account", new { email = user.Email });
                }
                else
                {
                    ModelState.AddModelError("", "Şifrenizin değiştirilme işleminde beklenmedik bir hata oluştu!Tekrar deneyiniz!");
                    return View(model);
                }

            }
            catch (Exception ex)
            {
                //ex loglanacak
                ModelState.AddModelError("", "Beklenmedik bir hata oluştu!Tekrar deneyiniz!.");
                return View(model);
            }
        }

    }
}
