using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LynxPMCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LynxPMCore.Controllers
{
    public class DataController : Controller
    {
        private LynxPMContext _context;
        public DataController (LynxPMContext context)
        {
            _context = context;
        }

      

        //public List<Area> areasList = new List<Area>();

        public IActionResult Index()
        {
            ViewBag.areas = _context.Areas.ToList();
            
            
            
        return View();
        }

        //public JsonResult getequipareabyID(int id)
        //{
        //    List<EquipmentArea> list = new List<EquipmentArea>();
        //    list = _context.EquipmentAreas.Where(ea => ea.Area.AreaID == id).ToList();
        //    list.Insert(0, new EquipmentArea { EquipmentAreaID = Guid.NewGuid(), EquipmentAreaName = "Select" });
        //    return Json(new SelectList(list, "EquipmentAreaID", "EquipmentAreaName"));
        //}
    }
}