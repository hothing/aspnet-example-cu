using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using ContosoUniversity.Models;
using ContosoUniversity.Models.ViewModels;

namespace ContosoUniversity.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Instructor> Instructor { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }
        public DbSet<CourseAssignment> CourseAssignments { get; set; }
        public DbSet<Person> People { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.Entity<Course>().ToTable("Course");
            
            var tEnrollemnt = modelBuilder.Entity<Enrollment>();
            tEnrollemnt.HasIndex(p => new { p.CourseID, p.StudentID }).IsUnique();
            tEnrollemnt.ToTable("Enrollment");
            
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Department>().ToTable("Department");
            modelBuilder.Entity<Instructor>().ToTable("Instructor");
            modelBuilder.Entity<OfficeAssignment>().ToTable("OfficeAssignment");
            modelBuilder.Entity<CourseAssignment>().ToTable("CourseAssignment");
            modelBuilder.Entity<Person>().ToTable("Person");
            modelBuilder.Entity<CourseAssignment>()
                .HasKey(c => new { c.CourseID, c.InstructorID });
        }

        public bool PersonExists(int id)
        {
            return People.Any(e => e.ID == id);
        }

        public bool StudentExists(int id)
        {
            return Students.Any(e => e.ID == id);
        }        

        public bool CourseExists(int id)
        {
            return Courses.Any(e => e.CourseID == id);
        }

        public bool InstructorExists(int id)
        {
            return Instructor.Any(e => e.ID == id);
        }

        public bool DepartmentExists(int id)
        {
            return Department.Any(e => e.DepartmentID == id);
        }

        public bool EnrollmentExists(int EnrollmentID)
        {
            return Enrollments.Any(e => e.EnrollmentID == EnrollmentID);
        }

        public bool OfficeAssignmentExists(int id)
        {
            return OfficeAssignments.Any(e => e.InstructorID == id);
        }

        public bool OfficeAssignmentExists(OfficeAssignment officeAssignment)
        {
            if (officeAssignment !=null)
            {
                return OfficeAssignments.Any(e => 
                            (e.InstructorID == officeAssignment.InstructorID) 
                            && (String.Compare(e.Location, officeAssignment.Location) == 0));
            }
            else
            {
                // because there is no information the worst case action is using
                return true;
            }
        }

        public bool AnyOfficeAssignmentExists(OfficeAssignment officeAssignment)
        {
            if (officeAssignment != null)
            {
                return OfficeAssignments.Any(e => (e.InstructorID == officeAssignment.InstructorID));
            }
            else
            {
                // because there is no information the worst case action is using
                return true;
            }
        }

        public bool EnrollmentExists(int CourseID, int StudentID)
        {
            return Enrollments.Any(e => (e.CourseID == CourseID) && (e.StudentID == StudentID));
        }

        public bool CourseAssignmentExists(int CourseID, int InstructorID)
        {
            return CourseAssignments.Any(e => (e.CourseID == CourseID) && (e.InstructorID == InstructorID));
        }

        public List<AssignedCourseData> getCoursesMap(int InstructorID)
        {
            var mapCourses = new List<AssignedCourseData>();
            var allCourses = Courses.ToList();
            var assignedCourses = CourseAssignments
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
    }
}