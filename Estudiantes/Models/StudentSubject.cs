using System;
using System.ComponentModel.DataAnnotations;

namespace Estudiantes.Models
{
    public class StudentSubject
    {
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public Student Student { get; set; }
        public Subject Subject { get; set; }
        public int Year {get; set;}
        [Display(Name = "Inscripcion")]
        public DateTime DateInscription { get; set; }
    }
}
