using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estudiantes.Models
{
    public class Career
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public List<Student> Students { get; set; }
    }
}
