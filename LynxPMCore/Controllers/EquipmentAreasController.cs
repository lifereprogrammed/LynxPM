using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LynxPMCore.Models;

namespace LynxPMCore.Controllers
{
    public class EquipmentAreasController : Controller
    {
        private readonly LynxPMContext _context;

        public EquipmentAreasController(LynxPMContext context)
        {
            _context = context;
        }

        // GET: EquipmentAreas
        public async Task<IActionResult> Index()
        {
            var lynxPMContext = _context.EquipmentAreas.Include(e => e.Area);
            return View(await lynxPMContext.ToListAsync());
        }

        // GET: EquipmentAreas/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipmentArea = await _context.EquipmentAreas
                .Include(e => e.Area)
                .SingleOrDefaultAsync(m => m.EquipmentAreaID == id);
            if (equipmentArea == null)
            {
                return NotFound();
            }

            return View(equipmentArea);
        }

        // GET: EquipmentAreas/Create
        public IActionResult Create()
        {
            ViewData["AreaID"] = new SelectList(_context.Areas, "AreaID", "AreaName");
            return View();
        }

        // POST: EquipmentAreas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EquipmentAreaID,EquipmentAreaName,EquipmentAreaDescription,AreaID,EquipmentAreaAppearanceOrder,EquipmentAreaActive")] EquipmentArea equipmentArea)
        {
            if (ModelState.IsValid)
            {
                equipmentArea.EquipmentAreaID = Guid.NewGuid();
                _context.Add(equipmentArea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AreaID"] = new SelectList(_context.Areas, "AreaID", "AreaName", equipmentArea.AreaID);
            return View(equipmentArea);
        }

        public JsonResult getequipareabyID(Guid id)
        {
            List<EquipmentArea> list = new List<EquipmentArea>();
            list = _context.EquipmentAreas.Where(ea => ea.Area.AreaID == id).ToList();
            list.Insert(0, new EquipmentArea { EquipmentAreaID = Guid.NewGuid(), EquipmentAreaName = "Select" });
            return Json(new SelectList(list, "EquipmentAreaID", "EquipmentAreaName"));
        }

        // GET: EquipmentAreas/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipmentArea = await _context.EquipmentAreas.SingleOrDefaultAsync(m => m.EquipmentAreaID == id);
            if (equipmentArea == null)
            {
                return NotFound();
            }
            ViewData["AreaID"] = new SelectList(_context.Areas, "AreaID", "AreaName", equipmentArea.AreaID);
            return View(equipmentArea);
        }

        // POST: EquipmentAreas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("EquipmentAreaID,EquipmentAreaName,EquipmentAreaDescription,AreaID,EquipmentAreaAppearanceOrder,EquipmentAreaActive")] EquipmentArea equipmentArea)
        {
            if (id != equipmentArea.EquipmentAreaID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(equipmentArea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquipmentAreaExists(equipmentArea.EquipmentAreaID))
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
            ViewData["AreaID"] = new SelectList(_context.Areas, "AreaID", "AreaName", equipmentArea.AreaID);
            return View(equipmentArea);
        }

        // GET: EquipmentAreas/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipmentArea = await _context.EquipmentAreas
                .Include(e => e.Area)
                .SingleOrDefaultAsync(m => m.EquipmentAreaID == id);
            if (equipmentArea == null)
            {
                return NotFound();
            }

            return View(equipmentArea);
        }

        // POST: EquipmentAreas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var equipmentArea = await _context.EquipmentAreas.SingleOrDefaultAsync(m => m.EquipmentAreaID == id);
            _context.EquipmentAreas.Remove(equipmentArea);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EquipmentAreaExists(Guid id)
        {
            return _context.EquipmentAreas.Any(e => e.EquipmentAreaID == id);
        }
    }
}
