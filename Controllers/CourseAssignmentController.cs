using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace cu_pum.Controllers
{
    public class CourseAssignmentController : Controller
    {
        private readonly SchoolContext _context;
        private readonly ILogger<CourseAssignmentController> _logger;

        public CourseAssignmentController(SchoolContext context, ILogger<CourseAssignmentController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: CourseAssignment
        public async Task<IActionResult> Index()
        {
            var schoolContext = _context.CourseAssignments.Include(c => c.Course).Include(c => c.Instructor);
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "Title");
            return View(await schoolContext.ToListAsync());
        }

        // GET: CourseAssignment/Details?CourseID=5&InstructorID=9
        public async Task<IActionResult> Details(int CourseID, int InstructorID)
        {
            var courseAssignment = await _context.CourseAssignments
                .Include(c => c.Course)
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(m => (m.CourseID == CourseID) && (m.InstructorID == InstructorID));
            if (courseAssignment == null)
            {
                return NotFound();
            }

            return View(courseAssignment);
        }

        // GET: CourseAssignment/Create
        public IActionResult Create()
        {
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "Title");
            ViewData["InstructorID"] = new SelectList(_context.Instructor, "ID", "FirstMidName");
            return View();
        }

        // POST: CourseAssignment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstructorID,CourseID")] CourseAssignment courseAssignment)
        {
            if (ModelState.IsValid)
            {
                // Step 1: check that pair (CourseID, InstructorID) is not exist
                var caTest = await _context.CourseAssignments.FindAsync(courseAssignment.CourseID, courseAssignment.InstructorID);
                if (caTest == null)
                {
                    // Step2 : ok, the record can be added
                    _context.Add(courseAssignment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }                
            }
            // not ok, the record exists. The actual page must be showen
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "Title", courseAssignment.CourseID);
            ViewData["InstructorID"] = new SelectList(_context.Instructor, "ID", "FirstMidName", courseAssignment.InstructorID);
            ViewData["Warning"] = "That course assigment already exists";
            return View(courseAssignment);
        }

         // GET: CourseAssignment/Edit?CourseID=5&InstructorID=9
        public async Task<IActionResult> Edit(int CourseID, int InstructorID)
        {
            var courseAssignment = await _context.CourseAssignments.FindAsync(CourseID, InstructorID);
            if (courseAssignment == null)
            {
                return NotFound();
            }
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "Title", courseAssignment.CourseID);
            ViewData["InstructorID"] = new SelectList(_context.Instructor, "ID", "FirstMidName", courseAssignment.InstructorID);
            return View(courseAssignment);
        }
        // POST: CourseAssignment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // 
        //public async Task<IActionResult> Edit(int id, [Bind("InstructorID, CourseID")] CourseAssignment courseAssignment)
        public async Task<IActionResult> Edit(int[] CourseID, int[] InstructorID)
        {
            /* _logger.LogInformation("POST id = {Id}, CourseID = {CourseID}, InstructorID = {CourseID}", id, CourseID, InstructorID);
            if ((CourseID != courseAssignment.CourseID) || (InstructorID != courseAssignment.InstructorID))
            {
                return NotFound();
            } */
            _logger.LogInformation("POST Old CourseID = {CourseID}, InstructorID = {CourseID}", CourseID[0], InstructorID[0]);
            _logger.LogInformation("POST New CourseID = {CourseID}, InstructorID = {CourseID}", CourseID[1], InstructorID[1]);
            var courseAssignment = await _context.CourseAssignments
                                        .Include(c => c.Course)
                                        .Include(c => c.Instructor)
                                        .FirstOrDefaultAsync(m => (m.CourseID == CourseID[0]) && (m.InstructorID == InstructorID[0]));
            
            if (ModelState.IsValid)
            {
                _logger.LogInformation("POST. Model is valid and will be updated");
                try
                {   
                    // The code below is not valid because:
                    // The property 'CourseAssignment.InstructorID' is part of a key and so cannot be modified or marked as modified. 
                    // To change the principal of an existing entity with an identifying foreign key, first delete the dependent and invoke 'SaveChanges', 
                    // and then associate the dependent with the new principal.   
                    /* courseAssignment.CourseID = CourseID[1];
                    courseAssignment.InstructorID = InstructorID[1];              
                    _context.Update(courseAssignment);
                    _logger.LogTrace(_context.ChangeTracker.DebugView.LongView);
                    if (!_context.ChangeTracker.HasChanges()) {_logger.LogWarning("data is up-to-dated (unexpected)"); } 
                    */
                    // Step 1: remove old record
                    _context.CourseAssignments.Remove(courseAssignment);
                    await _context.SaveChangesAsync();
                    // Step 2a: create new course assigment
                    var couseAssigment = new CourseAssignment() { CourseID = CourseID[1], InstructorID = InstructorID[1] } ; 
                    // Step 2b: check that pair (CourseID, InstructorID) is not exist
                    var caTest = await _context.CourseAssignments.FindAsync(courseAssignment.CourseID, courseAssignment.InstructorID);
                    if (caTest == null)
                    {
                        // Step 3 : ok, the record can be added
                        _context.Add(courseAssignment);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }                                        
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseAssignmentExists(courseAssignment.CourseID, courseAssignment.InstructorID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "Title", courseAssignment.CourseID);
            ViewData["InstructorID"] = new SelectList(_context.Instructor, "ID", "FirstMidName", courseAssignment.InstructorID);
            ViewData["Warning"] = "That course assigment already exists";
            return View(courseAssignment);
        }

        // GET: CourseAssignment/Delete?
        public async Task<IActionResult> Delete(int CourseID, int InstructorID)
        {
            var courseAssignment = await _context.CourseAssignments
                .Include(c => c.Course)
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(m => (m.CourseID == CourseID) && (m.InstructorID == InstructorID));
            if (courseAssignment == null)
            {
                return NotFound();
            }

            return View(courseAssignment);
        }

        // POST: CourseAssignment/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int CourseID, int InstructorID)
        {
            var courseAssignment = await _context.CourseAssignments.FindAsync(CourseID, InstructorID);
            _context.CourseAssignments.Remove(courseAssignment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseAssignmentExists(int CourseID, int InstructorID)
        {
            return _context.CourseAssignments.Any(e => (e.CourseID == CourseID) && (e.InstructorID == InstructorID));
        }
    }
}
