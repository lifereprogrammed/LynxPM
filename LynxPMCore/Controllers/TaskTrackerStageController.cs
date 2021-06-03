using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LynxPMCore.Models;

namespace LynxPMCore.Controllers
{
    public class TaskTrackerStageController : Controller
    {
        private readonly LynxPMContext _context;

        public TaskTrackerStageController (LynxPMContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}