using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace cu_pum.Controllers
{
    public class OfficeAssignmentController : Controller
    {
        private readonly SchoolContext _context;

        public OfficeAssignmentController(SchoolContext context)
        {
            _context = context;
        }

        // GET: OfficeAssignment
        public async Task<IActionResult> Index()
        {
            var schoolContext = _context.OfficeAssignments.Include(o => o.Instructor);
            return View(await schoolContext.ToListAsync());
        }

        // GET: OfficeAssignment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var officeAssignment = await _context.OfficeAssignments
                .Include(o => o.Instructor)
                .FirstOrDefaultAsync(m => m.InstructorID == id);
            if (officeAssignment == null)
            {
                return NotFound();
            }

            return View(officeAssignment);
        }

        // GET: OfficeAssignment/Create
        public IActionResult Create()
        {
            MakeInstructorList(null);
            return View();
        }

        // POST: OfficeAssignment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstructorID,Location")] OfficeAssignment officeAssignment)
        {
            if (ModelState.IsValid)
            {
                if (!OfficeAssignmentExists(officeAssignment))
                    {
                        _context.Add(officeAssignment);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        AssigmentWarning(officeAssignment);
                    }                
            }
            MakeInstructorList(officeAssignment);
            return View(officeAssignment);
        }

        // GET: OfficeAssignment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var officeAssignment = await _context.OfficeAssignments.FindAsync(id);
            if (officeAssignment == null)
            {
                return NotFound();
            }
            MakeInstructorList(officeAssignment);
            return View(officeAssignment);
        }

        // POST: OfficeAssignment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InstructorID,Location")] OfficeAssignment officeAssignment)
        {
            if (id != officeAssignment.InstructorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!OfficeAssignmentExists(officeAssignment))
                    {
                        _context.Update(officeAssignment);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        AssigmentWarning(officeAssignment);
                    }                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfficeAssignmentExists(officeAssignment.InstructorID))
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
            MakeInstructorList(officeAssignment);
            return View(officeAssignment);
        }

        // GET: OfficeAssignment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var officeAssignment = await _context.OfficeAssignments
                .Include(o => o.Instructor)
                .FirstOrDefaultAsync(m => m.InstructorID == id);
            if (officeAssignment == null)
            {
                return NotFound();
            }

            return View(officeAssignment);
        }

        // POST: OfficeAssignment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var officeAssignment = await _context.OfficeAssignments.FindAsync(id);
            _context.OfficeAssignments.Remove(officeAssignment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfficeAssignmentExists(int id)
        {
            return _context.OfficeAssignments.Any(e => e.InstructorID == id);
        }

        private bool OfficeAssignmentExists(OfficeAssignment officeAssignment)
        {
            if (officeAssignment !=null)
            {
                return _context.OfficeAssignments.Any(e => (e.InstructorID == officeAssignment.InstructorID) && (String.Compare(e.Location, officeAssignment.Location) == 0));
            }
            else
            {
                // because there is no information the worst case action is using
                return true;
            }
        }

        private void MakeInstructorList(OfficeAssignment officeAssignment)
        {
            ViewData["InstructorID"] = new SelectList(_context.Instructor, "ID", "FullName", officeAssignment?.InstructorID);
        }

        private void AssigmentWarning(OfficeAssignment officeAssignment)
        {
           ViewData["Warning"] = "That office assigment already exists";
        }
    }
}
