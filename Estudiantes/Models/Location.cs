using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estudiantes.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public int StudentId { get; set; }
    }
}
