using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.Models.ViewModels
{
    public class InstructorDetailsData
    {
        public Instructor Instructor { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
    }
}