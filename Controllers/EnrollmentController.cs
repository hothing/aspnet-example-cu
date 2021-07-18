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
    public class EnrollmentController : Controller
    {
        private readonly SchoolContext _context;

        public EnrollmentController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Enrollment
        public async Task<IActionResult> Index()
        {
            var schoolContext = _context.Enrollments.Include(e => e.Course).Include(e => e.Student);
            return View(await schoolContext.ToListAsync());
        }

        // GET: Enrollment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.EnrollmentID == id);
                if (enrollment != null)
                {
                    return View(enrollment);
                }                
            }            
            return NotFound();
        }

        // GET: Enrollment/Create
        public IActionResult Create()
        {
            MakeCourseList(null);
            MakeStudentList(null);
            return View();
        }

        // POST: Enrollment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EnrollmentID,CourseID,StudentID,Grade")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                if (!_context.EnrollmentExists(enrollment.CourseID, enrollment.StudentID))
                {
                    _context.Add(enrollment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Enrolement is already done");
                }                
            }
            MakeCourseList(enrollment);
            MakeStudentList(enrollment);
            return View(enrollment);
        }

        // GET: Enrollment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            MakeCourseList(enrollment);
            MakeStudentList(enrollment);
            return View(enrollment);
        }

        // POST: Enrollment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EnrollmentID,CourseID,StudentID,Grade")] Enrollment enrollment)
        {
            if (id != enrollment.EnrollmentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!_context.EnrollmentExists(enrollment.CourseID, enrollment.StudentID))
                    {
                        _context.Update(enrollment);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Enrolement is already done");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.EnrollmentExists(enrollment.EnrollmentID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }                
            }
            MakeCourseList(enrollment);
            MakeStudentList(enrollment);
            return View(enrollment);
        }

        // GET: Enrollment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.EnrollmentID == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // POST: Enrollment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        
        private void MakeCourseList(Enrollment enrollment)
        {
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "Title", enrollment?.CourseID);
        }

        private void MakeStudentList(Enrollment enrollment)
        {
            ViewData["StudentID"] = new SelectList(_context.Students, "ID", "FullName", enrollment?.StudentID);
        }
    }
}
