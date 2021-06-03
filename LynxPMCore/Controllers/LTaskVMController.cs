using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LynxPMCore.ViewModels;

namespace LynxPMCore.Controllers
{
    public class LTaskVMController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}