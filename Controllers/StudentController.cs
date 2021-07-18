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

namespace ContosoUniversity
{
    public class StudentController : Controller
    {
        private readonly SchoolContext _context;

        public StudentController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Student
        public async Task<IActionResult> Index()
        {
            return View(await _context.Students.ToListAsync());
        }

        // GET: Student/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {   
                var student = await _context.Students
                    .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                    .FirstOrDefaultAsync(m => m.ID == id);
                if (student != null)
                {
                    var studentDetails = new StudentDetailsData() {
                        Student = student,
                        Enrollments = student.Enrollments // FIXME: yes, it was stupid
                    };
                    return View(studentDetails);    
                }
            }
            return NotFound();
        }

        // GET: Student/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EnrollmentDate,ID,LastName,FirstMidName")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                var student = await _context.Students.FindAsync(id);
                if (student != null)
                {
                    await _context.Entry(student).Collection(s => s.Enrollments).LoadAsync();
                    var courses = await _context.Courses.ToListAsync();

                    var se = new StudentEditForm() {
                        ID = student.ID,
                        FirstMidName = student.FirstMidName,
                        LastName = student.LastName,
                        EnrollmentDate = student.EnrollmentDate,
                    };
                    var ecorses = new List<CourseEnroll>();                    
                    foreach (var c in courses)
                    {
                        var ec = new CourseEnroll(){
                            CourseID = c.CourseID,
                            Title = c.Title,
                            Grade = null,
                            Enrolled = false
                        };
                        
                        if (student.Enrollments != null)
                        {
                            var sac = student.Enrollments.Where(e => e.CourseID == c.CourseID).FirstOrDefault();
                            if (sac != null)
                            {
                                ec.Grade = sac.Grade;
                                ec.Enrolled = true;
                            }
                        }
                        ecorses.Add(ec);
                    }
                    se.Courses = ecorses;
                    return View(se);
                }
            }
            return NotFound();
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, 
            [Bind("EnrollmentDate,ID,LastName,FirstMidName")] Student student,
            int[] selectedCourses)
        {
            if (id == student.ID)
            {
                if (ModelState.IsValid)
                {
                    if (_context.StudentExists(student.ID))
                    try
                    {
                        //update primary student information
                        _context.Update(student);
                        //await _context.SaveChangesAsync();
                        // Enrollemnt in the courses
                        if (selectedCourses != null)
                        {
                            var _student = await _context.Students
                                                .Include(s => s.Enrollments)
                                                .FirstOrDefaultAsync(s => s.ID == id);
                            if (_student != null)
                            {
                                var allCourses = await _context.Courses.ToListAsync();
                                var enrollemnts = _student.Enrollments.ToList();
                                var updatedCourses = new List<int>(selectedCourses);
                                // stage 1: remove assigment
                                foreach (var ecourse in enrollemnts)
                                {
                                    if (!updatedCourses.Contains(ecourse.CourseID))
                                    {
                                        var x = allCourses.Find(c => c.CourseID == ecourse.CourseID);
                                        if (x != null)
                                        {
                                            _context.Enrollments.Remove(ecourse);                                            
                                        }
                                        updatedCourses.Remove(ecourse.CourseID);                                   
                                    } 
                                }
                                // stage 2: add assigment
                                foreach (var newCourse in updatedCourses)
                                {
                                    if (!_context.EnrollmentExists(newCourse, id))
                                    {   
                                        _context.Add(new Enrollment() 
                                                    { 
                                                        CourseID = newCourse, 
                                                        StudentID = id
                                                    });
                                    }
                                }
                                                               
                            }                            
                        }
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (_context.StudentExists(student.ID))
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(student);
            }
            return NotFound();
        }

        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }        
    }
}
