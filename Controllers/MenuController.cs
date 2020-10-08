using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TP1_KarineDunberry.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult TelechargerMenu()
        {
            return File("~/Documents/ZhaoMenu.pdf", "application/pdf");
        }

        public IActionResult TelechargerCarteVins()
        {
            return File("~/Documents/ZhaoCarteVins.pdf", "application/pdf");
        }
    }
}
