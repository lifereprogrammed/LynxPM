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
    public class DueStatusController : Controller
    {
        private readonly LynxPMContext _context;

        public DueStatusController(LynxPMContext context)
        {
            _context = context;
        }

        // GET: DueStatus
        public async Task<IActionResult> Index()
        {
            return View(await _context.DueStatuses.ToListAsync());
        }

        // GET: DueStatus/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dueStatus = await _context.DueStatuses
                .SingleOrDefaultAsync(m => m.DueStatusID == id);
            if (dueStatus == null)
            {
                return NotFound();
            }

            return View(dueStatus);
        }

        // GET: DueStatus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DueStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DueStatusID,DueStatusName,DustStatusDescription")] DueStatus dueStatus)
        {
            if (ModelState.IsValid)
            {
                dueStatus.DueStatusID = Guid.NewGuid();
                _context.Add(dueStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dueStatus);
        }

        // GET: DueStatus/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dueStatus = await _context.DueStatuses.SingleOrDefaultAsync(m => m.DueStatusID == id);
            if (dueStatus == null)
            {
                return NotFound();
            }
            return View(dueStatus);
        }

        // POST: DueStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("DueStatusID,DueStatusName,DustStatusDescription")] DueStatus dueStatus)
        {
            if (id != dueStatus.DueStatusID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dueStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DueStatusExists(dueStatus.DueStatusID))
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
            return View(dueStatus);
        }

        // GET: DueStatus/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dueStatus = await _context.DueStatuses
                .SingleOrDefaultAsync(m => m.DueStatusID == id);
            if (dueStatus == null)
            {
                return NotFound();
            }

            return View(dueStatus);
        }

        // POST: DueStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var dueStatus = await _context.DueStatuses.SingleOrDefaultAsync(m => m.DueStatusID == id);
            _context.DueStatuses.Remove(dueStatus);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DueStatusExists(Guid id)
        {
            return _context.DueStatuses.Any(e => e.DueStatusID == id);
        }
    }
}
