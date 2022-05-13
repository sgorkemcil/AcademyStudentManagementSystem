﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASMSBusinessLayer.ContractsBLL;

namespace ASMSPresentationLayer.Controllers
{
    public class NeighbourdhoodController : Controller
    {
        private readonly INeighbourhoodBusinessEngine _neighbourhoodEngine;

        public NeighbourdhoodController(INeighbourhoodBusinessEngine neighbourhoodEngine)
        {
            _neighbourhoodEngine = neighbourhoodEngine;
        }

        public JsonResult GetDistrictNeighbourhoods(int id)
        {
            try
            {
                var data = _neighbourhoodEngine.GetNeighbourhoodsOfDistrict(id).Data;
                return Json(new { isSuccess = true, data });
            }
            catch (Exception)
            {
                //ex loglanabilir
                return Json(new { isSuccess = false });
            }
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
