﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TP1_KarineDunberry.Controllers
{
    public class GalerieController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
