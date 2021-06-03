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
    public class WorkGroupsController : Controller
    {
        private readonly LynxPMContext _context;

        public WorkGroupsController(LynxPMContext context)
        {
            _context = context;
        }

        // GET: WorkGroups
        public async Task<IActionResult> Index()
        {
            return View(await _context.WorkGroups.ToListAsync());
        }

        // GET: WorkGroups/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workGroup = await _context.WorkGroups
                .SingleOrDefaultAsync(m => m.WorkGroupID == id);
            if (workGroup == null)
            {
                return NotFound();
            }

            return View(workGroup);
        }

        // GET: WorkGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WorkGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WorkGroupID,WorkGroupName,WorkGroupDrescription")] WorkGroup workGroup)
        {
            if (ModelState.IsValid)
            {
                workGroup.WorkGroupID = Guid.NewGuid();
                _context.Add(workGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(workGroup);
        }

        // GET: WorkGroups/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workGroup = await _context.WorkGroups.SingleOrDefaultAsync(m => m.WorkGroupID == id);
            if (workGroup == null)
            {
                return NotFound();
            }
            return View(workGroup);
        }

        // POST: WorkGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("WorkGroupID,WorkGroupName,WorkGroupDrescription")] WorkGroup workGroup)
        {
            if (id != workGroup.WorkGroupID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkGroupExists(workGroup.WorkGroupID))
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
            return View(workGroup);
        }

        // GET: WorkGroups/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workGroup = await _context.WorkGroups
                .SingleOrDefaultAsync(m => m.WorkGroupID == id);
            if (workGroup == null)
            {
                return NotFound();
            }

            return View(workGroup);
        }

        // POST: WorkGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var workGroup = await _context.WorkGroups.SingleOrDefaultAsync(m => m.WorkGroupID == id);
            _context.WorkGroups.Remove(workGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkGroupExists(Guid id)
        {
            return _context.WorkGroups.Any(e => e.WorkGroupID == id);
        }
    }
}
