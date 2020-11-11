using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estudiantes.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Year { get; set; }
        public Location Location { get; set; }
        public int? CareerId { get; set; }
        public Career Career { get; set; }
        public List<StudentSubject> StudentSubjects { get; set; }
        public string Picture { get; set; }
    }
}
