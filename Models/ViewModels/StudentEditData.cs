using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models.ViewModels
{
    public class StudentEditForm
    {
        public int ID { get; set; }
        
        [MinLength(3)]
        public string FirstMidName { get; set; }

        [MinLength(2)]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hire Date")]
        public DateTime EnrollmentDate { get; set; }
        
        public IEnumerable<CourseEnroll> Courses { get; set; } // to select and enroll in the course

    }

    public class CourseEnroll
    {
        public int CourseID { get; set; }

        public string Title { get; set; }

        [DisplayFormat(NullDisplayText = "No grade")]
        public Grade? Grade { get; set; }

        public bool Enrolled { get; set; }

    }
}