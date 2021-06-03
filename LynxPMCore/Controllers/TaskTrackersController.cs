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
    public class TaskTrackersController : Controller
    {
        private readonly LynxPMContext _context;

        public TaskTrackersController(LynxPMContext context)
        {
            _context = context;
        }

        // GET: TaskTrackers
        public async Task<IActionResult> Index()
        {
            var lynxTTrack = _context.TaskTrackers
                .Include(tt => tt.LTask)
                .Include(tt => tt.Condition)
                .AsNoTracking();
                return View(await lynxTTrack.ToListAsync());
        }

        // GET: TaskTrackers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskTracker = await _context.TaskTrackers
                .SingleOrDefaultAsync(m => m.TaskTrackerID == id);
            if (taskTracker == null)
            {
                return NotFound();
            }

            return View(taskTracker);
        }

        // GET: TaskTrackers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TaskTrackers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaskTrackerID,TaskID,TaskTrackerComments,TaskTrackerCompletionDate,TaskTrackerPreviousCompletionDate,TaskTrackerExpectedCompletionDate,TaskTrackerDaystoComplete,ConditionID,TaskTrackerRecordUser,TaskTrackerDateStamp,TaskRecordUserIP")] TaskTracker taskTracker)
        {
            if (ModelState.IsValid)
            {
                taskTracker.TaskTrackerID = Guid.NewGuid();
                _context.Add(taskTracker);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taskTracker);
        }

        // GET: TaskTrackers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskTracker = await _context.TaskTrackers.SingleOrDefaultAsync(m => m.TaskTrackerID == id);
            if (taskTracker == null)
            {
                return NotFound();
            }
            return View(taskTracker);
        }

        // POST: TaskTrackers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TaskTrackerID,TaskID,TaskTrackerComments,TaskTrackerCompletionDate,TaskTrackerPreviousCompletionDate,TaskTrackerExpectedCompletionDate,TaskTrackerDaystoComplete,ConditionID,TaskTrackerRecordUser,TaskTrackerDateStamp,TaskRecordUserIP")] TaskTracker taskTracker)
        {
            if (id != taskTracker.TaskTrackerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskTracker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskTrackerExists(taskTracker.TaskTrackerID))
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
            return View(taskTracker);
        }

        // GET: TaskTrackers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskTracker = await _context.TaskTrackers
                .SingleOrDefaultAsync(m => m.TaskTrackerID == id);
            if (taskTracker == null)
            {
                return NotFound();
            }

            return View(taskTracker);
        }

        // POST: TaskTrackers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var taskTracker = await _context.TaskTrackers.SingleOrDefaultAsync(m => m.TaskTrackerID == id);
            _context.TaskTrackers.Remove(taskTracker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskTrackerExists(Guid id)
        {
            return _context.TaskTrackers.Any(e => e.TaskTrackerID == id);
        }
    }
}
