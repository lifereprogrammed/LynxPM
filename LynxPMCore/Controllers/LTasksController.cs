using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LynxPMCore.Models;
using System.Reflection;

namespace LynxPMCore.Controllers
{
    public class LTasksController : Controller
    {
        private readonly LynxPMContext _context;


        public LTasksController(LynxPMContext context)
        {
            _context = context;
        }

        // GET: LTasks
        public async Task<IActionResult> Index()
        {
            var lynxTasks = _context.LTasks
                .Include(lt => lt.Term)
                .Include(lt => lt.Equipment)
                .Include(lt => lt.TaskType)
                .AsNoTracking();
            return View(await lynxTasks.ToListAsync());
            //return View(await _context.LTasks.ToListAsync());
        }

        // GET: LTasks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lTask = await _context.LTasks
                .SingleOrDefaultAsync(m => m.LTaskID == id);
            if (lTask == null)
            {
                return NotFound();
            }

            return View(lTask);
        }

        // GET: LTasks/Create
        public IActionResult Create()
        {
            ViewData["AreaID"] = new SelectList(_context.Areas, "AreaID", "AreaName");

            ViewData["EquipmentAreaID"] = new SelectList(_context.EquipmentAreas, "EquipmentAreaID", "EquipmentAreaName");
            

            ViewData["EquipmentID"] = new SelectList(_context.Equipments, "EquipmentID", "EquipmentName");
            ViewData["TermID"] = new SelectList(_context.Terms, "TermID", "TermName");
            ViewData["TaskTypeID"] = new SelectList(_context.TaskTypes, "TaskTypeID", "TaskTypeName");
            return View();
        }

        public JsonResult getequipareabyID(Guid id)
        {
            List<EquipmentArea> list = new List<EquipmentArea>();
            list = _context.EquipmentAreas.Where(ea => ea.Area.AreaID == id).ToList();
            list.Insert(0, new EquipmentArea { EquipmentAreaID = Guid.NewGuid(), EquipmentAreaName = "Select" });
            return Json(new SelectList(list, "EquipmentAreaID", "EquipmentAreaName"));
        }

        public JsonResult getequipbyID(Guid id)
        {
            List<Equipment> list = new List<Equipment>();
            list = _context.Equipments.Where(eq => eq.EquipmentArea.EquipmentAreaID == id).ToList();
            list.Insert(0, new Equipment { EquipmentID = Guid.NewGuid(), EquipmentName = "Select" });
            return Json(new SelectList(list, "EquipmentID", "EquipmentName"));
        }

        public JsonResult getequipbyAreaID(Guid id)
        {
            List<Equipment> equiplist = new List<Equipment>();
            List<EquipmentArea> eaList = new List<EquipmentArea>();

            eaList = _context.EquipmentAreas.Where(ea => ea.Area.AreaID == id).ToList();
            
            equiplist = _context.Equipments.Where(eq => eq.EquipmentArea.AreaID == id).ToList();
            equiplist.Insert(0, new Equipment { EquipmentID = Guid.NewGuid(), EquipmentName = "Select" });

            return Json(new SelectList(equiplist, "EquipmentID", "EquipmentName"));
        }

        // POST: LTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LTaskID,LTaskName,LTaskDescription,TermID,EquipmentID,TaskTypeID")] LTask lTask)
        {
            if (ModelState.IsValid)
            {
                lTask.LTaskID = Guid.NewGuid();
                _context.Add(lTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["AreaID"] = new SelectList(_context.Areas, "AreaID", "AreaName");
            ViewBag.Area = ViewData["AreaID"];
            ViewData["EquipmentAreaID"] = new SelectList(_context.EquipmentAreas, "EquipmentAreaID", "EquipmentAreaName");
            ViewBag.EquipmentArea = ViewData["EquipmentAreaID"];
            ViewData["EquipmentID"] = new SelectList(_context.Equipments, "EquipmentID", "EquipmentName");
            ViewData["TermID"] = new SelectList(_context.Terms, "TermID", "TermName");
            ViewData["TaskTypeID"] = new SelectList(_context.TaskTypes, "TaskTypeID", "TaskTypeName");

            return View(lTask);
        }

        // GET: LTasks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lTask = await _context.LTasks.SingleOrDefaultAsync(m => m.LTaskID == id);
            if (lTask == null)
            {
                return NotFound();
            }

            ViewData["AreaID"] = new SelectList(_context.Areas, "AreaID", "AreaName");
            ViewData["EquipmentAreaID"] = new SelectList(_context.EquipmentAreas, "EquipmentAreaID", "EquipmentAreaName");
            ViewData["EquipmentID"] = new SelectList(_context.Equipments, "EquipmentID", "EquipmentName");
            ViewData["TermID"] = new SelectList(_context.Terms, "TermID", "TermName");
            ViewData["TaskTypeID"] = new SelectList(_context.TaskTypes, "TaskTypeID", "TaskTypeName");
            return View(lTask);
        }

        // POST: LTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("LTaskID,LTaskName,LTaskDescription,TermID,EquipmentID,TaskTypeID")] LTask lTask)
        {
            if (id != lTask.LTaskID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LTaskExists(lTask.LTaskID))
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

            ViewData["AreaID"] = new SelectList(_context.Areas, "AreaID", "AreaName");
            ViewData["EquipmentAreaID"] = new SelectList(_context.EquipmentAreas, "EquipmentAreaID", "EquipmentAreaName");
            ViewData["EquipmentID"] = new SelectList(_context.Equipments, "EquipmentID", "EquipmentName");
            ViewData["TermID"] = new SelectList(_context.Terms, "TermID", "TermName");
            ViewData["TaskTypeID"] = new SelectList(_context.TaskTypes, "TaskTypeID", "TaskTypeName");
            return View(lTask);
        }

        // GET: LTasks/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lTask = await _context.LTasks
                .SingleOrDefaultAsync(m => m.LTaskID == id);
            if (lTask == null)
            {
                return NotFound();
            }

            return View(lTask);
        }

        // POST: LTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var lTask = await _context.LTasks.SingleOrDefaultAsync(m => m.LTaskID == id);
            _context.LTasks.Remove(lTask);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LTaskExists(Guid id)
        {
            return _context.LTasks.Any(e => e.LTaskID == id);
        }
    }
}
