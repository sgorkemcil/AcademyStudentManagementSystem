﻿using ASMSBusinessLayer.ContractsBLL;
using ASMSBusinessLayer.EmailService;
using ASMSEntityLayer.IdentityModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASMSPresentationLayer.Controllers
{
    [Authorize]//Bu sayede sadece login olan kullanıcılar buraya gelecek
    public class AddressController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IUsersAddressBusinessEngine _userAddress;
        private readonly ICityBusinessEngine _cityEngine;

        public AddressController(UserManager<AppUser> userManager, IEmailSender emailSender,
            IUsersAddressBusinessEngine userAddress, ICityBusinessEngine cityEngine)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _userAddress = userAddress;
            _cityEngine = cityEngine;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddAddress()
        {
            //İlleri sayfaya götürsün
            ViewBag.Cities = _cityEngine.GetAll().Data;
            return View();
        }
    }
}