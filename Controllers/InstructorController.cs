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
                    ViewData["Course"] = selectedCourse.Title;
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
            var courses = await _context.Courses.ToListAsync();
            var instructor = await _context.Instructor
                .Include(instr => instr.CourseAssignments)
                .ThenInclude(ca => ca.Course)
                .Include(instr => instr.OfficeAssignment)
                .FirstOrDefaultAsync(m => m.ID == id);
                
            if (instructor != null)
            {
                var editInstructor = new InstructorEditForm() {
                    ID = instructor.ID,
                    FirstMidName = instructor.FirstMidName,
                    LastName = instructor.LastName,
                    HireDate = instructor.HireDate,
                    Office = instructor.OfficeAssignment.Location,
                    Courses = getCoursesMap(instructor.ID) 
                };
                return View(editInstructor);   
            }
            else
            {
                return NotFound();
            }
        }

        // POST: Instructor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, 
                                                [Bind("ID,HireDate,LastName,FirstMidName,Office,selectedCourses")] 
                                                InstructorEditResponse response)
        {
            if (id != response.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {                    
                    if (ModelState.IsValid)
                    {
                        var instructor =new Instructor() {
                            ID = response.ID,
                            FirstMidName = response.FirstMidName,
                            LastName = response.LastName,
                            HireDate = response.HireDate
                        };
                        var officeAssignment = new OfficeAssignment() {
                            InstructorID = response.ID,
                            Location = response.Office
                        };
                        _context.Update(officeAssignment);
                        _context.Update(instructor);
                        // TODO:Course Assigment       
                        await _context.SaveChangesAsync();                
                    }
                    else
                    {
                        // not ok, the record exists. The actual page must be showen with warning
                        //ModelState.AddModelError(nameof(response.selectedCourses), "Assigment exists");
                    }                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorExists(response.ID))
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
            
            return View( new InstructorEditForm() {
                ID = response.ID,
                FirstMidName = response.FirstMidName,
                LastName = response.LastName,
                HireDate = response.HireDate,
                Office = response.Office,
                Courses = getCoursesMap(response.ID) 
            });
        }

        private bool CourseAssignmentExists(int CourseID, int InstructorID)
        {
            return _context.CourseAssignments.Any(e => (e.CourseID == CourseID) && (e.InstructorID == InstructorID));
        }

        private List<AssignedCourseData> getCoursesMap(int InstructorID)
        {
            var mapCourses = new List<AssignedCourseData>();
            var allCourses = _context.Courses.ToList();
            var assignedCourses = _context.CourseAssignments
                                    .Include(c => c.Course)
                                    .Where(ca => ca.InstructorID == InstructorID)
                                    .ToList();
            foreach (var c in allCourses)
            {
                var r = assignedCourses.Where(ca => ca.CourseID == c.CourseID).Count();
                mapCourses.Add(new AssignedCourseData() {
                    CourseID = c.CourseID,
                    Title = c.Title,
                    Assigned = (r > 0)
                });
            }
            return mapCourses;
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
