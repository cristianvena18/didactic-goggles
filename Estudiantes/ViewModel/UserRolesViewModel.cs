using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estudiantes.ViewModel
{
    public class UserRolesViewModel
    {
        public string UserId { get; internal set; }
        public string UserName { get; internal set; }
        public string Email { get; internal set; }
        public List<string> Roles { get; internal set; }
    }
}
