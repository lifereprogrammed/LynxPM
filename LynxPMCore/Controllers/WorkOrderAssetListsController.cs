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
    public class WorkOrderAssetListsController : Controller
    {
        private readonly LynxPMContext _context;

        public WorkOrderAssetListsController(LynxPMContext context)
        {
            _context = context;
        }

        // GET: WorkOrderAssetLists
        public async Task<IActionResult> Index()
        {
            var lynxPMContext = _context.WorkOrderAssetsLists.Include(w => w.Equipment);
            return View(await lynxPMContext.ToListAsync());
        }

        // GET: WorkOrderAssetLists/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workOrderAssetList = await _context.WorkOrderAssetsLists
                .Include(w => w.Equipment)
                .SingleOrDefaultAsync(m => m.WorkOrderAssetListID == id);
            if (workOrderAssetList == null)
            {
                return NotFound();
            }

            return View(workOrderAssetList);
        }

        // GET: WorkOrderAssetLists/Create
        public IActionResult Create()
        {
            ViewData["EquipmentID"] = new SelectList(_context.Equipments, "EquipmentID", "EquipmentID");
            return View();
        }

        // POST: WorkOrderAssetLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WorkOrderAssetListID,WorkOderID,EquipmentID")] WorkOrderAssetList workOrderAssetList)
        {
            if (ModelState.IsValid)
            {
                workOrderAssetList.WorkOrderAssetListID = Guid.NewGuid();
                _context.Add(workOrderAssetList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EquipmentID"] = new SelectList(_context.Equipments, "EquipmentID", "EquipmentID", workOrderAssetList.EquipmentID);
            return View(workOrderAssetList);
        }

        // GET: WorkOrderAssetLists/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workOrderAssetList = await _context.WorkOrderAssetsLists.SingleOrDefaultAsync(m => m.WorkOrderAssetListID == id);
            if (workOrderAssetList == null)
            {
                return NotFound();
            }
            ViewData["EquipmentID"] = new SelectList(_context.Equipments, "EquipmentID", "EquipmentID", workOrderAssetList.EquipmentID);
            return View(workOrderAssetList);
        }

        // POST: WorkOrderAssetLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("WorkOrderAssetListID,WorkOderID,EquipmentID")] WorkOrderAssetList workOrderAssetList)
        {
            if (id != workOrderAssetList.WorkOrderAssetListID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workOrderAssetList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkOrderAssetListExists(workOrderAssetList.WorkOrderAssetListID))
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
            ViewData["EquipmentID"] = new SelectList(_context.Equipments, "EquipmentID", "EquipmentID", workOrderAssetList.EquipmentID);
            return View(workOrderAssetList);
        }

        // GET: WorkOrderAssetLists/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workOrderAssetList = await _context.WorkOrderAssetsLists
                .Include(w => w.Equipment)
                .SingleOrDefaultAsync(m => m.WorkOrderAssetListID == id);
            if (workOrderAssetList == null)
            {
                return NotFound();
            }

            return View(workOrderAssetList);
        }

        // POST: WorkOrderAssetLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var workOrderAssetList = await _context.WorkOrderAssetsLists.SingleOrDefaultAsync(m => m.WorkOrderAssetListID == id);
            _context.WorkOrderAssetsLists.Remove(workOrderAssetList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkOrderAssetListExists(Guid id)
        {
            return _context.WorkOrderAssetsLists.Any(e => e.WorkOrderAssetListID == id);
        }
    }
}
