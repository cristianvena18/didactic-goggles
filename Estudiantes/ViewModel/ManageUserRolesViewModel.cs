using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estudiantes.ViewModel
{
    public class ManageUserRolesViewModel
    {
        public string RoleId { get; internal set; }
        public string RoleName { get; internal set; }
        public bool IsSelected { get; internal set; }
    }
}
