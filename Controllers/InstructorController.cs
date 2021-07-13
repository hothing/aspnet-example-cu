using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Models.ViewModels;

namespace cu_pum.Controllers
{
    public class InstructorController : Controller
    {
        private readonly SchoolContext _context;

        public InstructorController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Instructor
        public async Task<IActionResult> Index()
        {
            var InstructorsList = await _context.Instructor
                        .Include(instr => instr.CourseAssignments)
                        .ThenInclude(ca => ca.Course)
                        .Include(instr => instr.OfficeAssignment)
                        .ToListAsync();
            /* foreach (var instr in InstructorsList)
            {
                _context.Entry(instr).Collection(b => b.CourseAssignments).Load();
            } */
            //return View(await _context.Instructor.ToListAsync());
            return View(InstructorsList);
        }

        // GET: Instructor/Details/5
        /* public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructor
                .Include(instr => instr.CourseAssignments)
                .ThenInclude(ca => ca.Course)
                .Include(instr => instr.OfficeAssignment)
                .FirstOrDefaultAsync(m => m.ID == id);
                
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        } */

        // GET: Instructor/Details/5
        public async Task<IActionResult> Details(int? id, int? CourseID)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructor
                .Include(instr => instr.CourseAssignments)
                .ThenInclude(ca => ca.Course)
                .Include(instr => instr.OfficeAssignment)
                .FirstOrDefaultAsync(m => m.ID == id);           

            if (instructor != null)
            {
                IEnumerable<Enrollment> ex = null;
                if (CourseID != null)
                {
                    var courseAssigment = instructor.CourseAssignments.Where(x => x.CourseID == CourseID).Single(); 
                    var selectedCourse = courseAssigment.Course;
                    await _context.Entry(selectedCourse).Collection(x => x.Enrollments).LoadAsync();
                    foreach (Enrollment enrollment in selectedCourse.Enrollments)
                    {
                        await _context.Entry(enrollment).Reference(x => x.Student).LoadAsync();
                    }
                    ex = selectedCourse.Enrollments;
                }
                var details = new InstructorDetailsData() {
                    Instructor = instructor,
                    Enrollments = ex
                };
                return View(details);
            }
            return NotFound();            
        }

        // GET: Instructor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Instructor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HireDate,ID,LastName,FirstMidName")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(instructor);
        }

        // GET: Instructor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var instructor = await _context.Instructor
            //    .FindAsync(id);
            var instructor = await _context.Instructor
                .Include(instr => instr.CourseAssignments)
                .ThenInclude(ca => ca.Course)
                .Include(instr => instr.OfficeAssignment)
                .FirstOrDefaultAsync(m => m.ID == id);
                
            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }

        // POST: Instructor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HireDate,ID,LastName,FirstMidName")] Instructor instructor)
        {
            if (id != instructor.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instructor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorExists(instructor.ID))
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
            return View(instructor);
        }

        // GET: Instructor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructor
                .FirstOrDefaultAsync(m => m.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructor = await _context.Instructor.FindAsync(id);
            _context.Instructor.Remove(instructor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructor.Any(e => e.ID == id);
        }
    }
}
