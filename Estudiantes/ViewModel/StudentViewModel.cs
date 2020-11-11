using Estudiantes.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estudiantes.ViewModel
{
    public class StudentViewModel
    {
        public List<Student> Students { get; set; }
        public SelectList ListCareers { get; set; }
        public string search { get; set; }
        public int? CareerId { get; set; }
        public Paginator Paginator { get; set; } = new Paginator();
    }

    public class Paginator
    {
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }
        public int ItemsByPage { get; set; }
        public int TotalPages => (int)Math.Ceiling((decimal)this.TotalItems / this.ItemsByPage);

        public Dictionary<string, string> QueryString { get; set; } = new Dictionary<string, string>();
    }
}
