using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models.ViewModels
{
    public class InstructorEditForm
    {
        public int ID { get; set; }
        
        [MinLength(3)]
        public string FirstMidName { get; set; }

        [MinLength(2)]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }

        [MinLength(3)]
        public string Office { get; set; } // to assign the office

        public IEnumerable<AssignedCourseData> Courses { get; set; } // to select and assign the course

    }


    public class InstructorEditResponse
    {
        public int ID { get; set; }
        public string FirstMidName { get; set; }

        public string LastName { get; set; }

        public DateTime HireDate { get; set; }

        public string Office { get; set; } // to assign the office

        public int[] selectedCourses { get; set; } // the assigned Courses

    }

    public class AssignedCourseData
    {
        public int CourseID { get; set; }
        public string Title { get; set; }
        public bool Assigned { get; set; }
    }
}