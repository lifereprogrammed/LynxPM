using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LynxPMCore.Models;
using System.Net;

namespace LynxPMCore.Controllers
{
    public class EquipmentsController : Controller
    {
        private readonly LynxPMContext _context;

        public EquipmentsController(LynxPMContext context)
        {
            _context = context;
        }

        // GET: Equipments
        public async Task<IActionResult> Index()
        {
            var eAs = _context.Equipments
                .Include(ea => ea.EquipmentArea)
                .AsNoTracking();
            //return View(await _context.Equipments.ToListAsync());
            return View(await eAs.ToListAsync());
        }

        //public List<Equipment> GetList()
        //{
        //    var eqs = _context.Equipments
        //            .Select(eq => new Equipment()
        //            {
        //                    EquipmentID = eq.EquipmentID,
        //                    EquipmentName = eq.EquipmentName,
        //                    EquipmentDescription = eq.EquipmentDescription,
        //                    EquipmentArea = eq.EquipmentArea
        //            });
        //    return eqs.ToList<Equipment>();
        //}


        // GET: Equipments/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipments
                .SingleOrDefaultAsync(m => m.EquipmentID == id);
            if (equipment == null)
            {
                return NotFound();
            }

            
            return View(equipment);
        }

        // GET: Equipments/Create
        public IActionResult Create()
        {
            ViewData["AreaID"] = new SelectList(_context.Areas, "AreaID", "AreaName");
            ViewData["EquipmentAreaID"] = new SelectList(_context.EquipmentAreas, "EquipmentAreaID", "EquipmentAreaName");
            return View();
        }

        public JsonResult getequipareabyID(Guid id)
        {
            List<EquipmentArea> list = new List<EquipmentArea>();
            list = _context.EquipmentAreas.Where(ea => ea.Area.AreaID == id).ToList();
            list.Insert(0, new EquipmentArea { EquipmentAreaID = Guid.NewGuid(), EquipmentAreaName = "Select" });
            return Json(new SelectList(list, "EquipmentAreaID", "EquipmentAreaName"));
        }

        public JsonResult getequiparea()
        {
            List<EquipmentArea> list = new List<EquipmentArea>();
            list = _context.EquipmentAreas.ToList();
            return Json(list);

        }

        // POST: Equipments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Create([Bind("EquipmentID,EquipmentName,EquipmentDescription,EquipmentAreaID,EquipmentAppearance,EquipmentActive,EquipmentPictureID,EquipmentPictureURL")] Equipment equipment )
        {
            if (ModelState.IsValid)
            {
                equipment.EquipmentID = Guid.NewGuid();
                _context.Add(equipment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //else
            //{
            //    var message = string.Join(" | ", ModelState.Values
            //        .SelectMany(v => v.Errors)
            //        .Select(e => e.ErrorMessage));
            //    ViewBag.message = message;
            //    return View(equipment);
            //    //return HTTPAlert(HttpStatusCode.BadRequest, message);
            //}
            ViewData["AreaID"] = new SelectList(_context.Areas, "AreaID", "AreaName", equipment.AreaID);

            ViewData["EquipmentAreaID"] = new SelectList(_context.EquipmentAreas, "EquipmentAreaID", "EquipmentAreaName", equipment.EquipmentAreaID);
            return View(equipment);
        }

        // GET: Equipments/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipments.SingleOrDefaultAsync(m => m.EquipmentID == id);
            if (equipment == null)
            {
                return NotFound();
            }

            ViewData["AreaID"] = new SelectList(_context.Areas, "AreaID", "AreaName");
            ViewData["EquipmentAreaID"] = new SelectList(_context.EquipmentAreas, "EquipmentAreaID", "EquipmentAreaName");

            return View(equipment);
        }

        // POST: Equipments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("EquipmentID,EquipmentName,EquipmentDescription,EquipmentAreaID,EquipmentAppearance,EquipmentActive,EquipmentPictureID,EquipmentPictureURL")] Equipment equipment)
        {
            if (id != equipment.EquipmentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(equipment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquipmentExists(equipment.EquipmentID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(equipment);
        }

        // GET: Equipments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipments
                .SingleOrDefaultAsync(m => m.EquipmentID == id);
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // POST: Equipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var equipment = await _context.Equipments.SingleOrDefaultAsync(m => m.EquipmentID == id);
            _context.Equipments.Remove(equipment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EquipmentExists(Guid id)
        {
            return _context.Equipments.Any(e => e.EquipmentID == id);
        }
    }
}
