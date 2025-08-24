using System;
using System.Collections.Generic;

namespace myFirstProject.Data.Models;

public partial class RolesPermiso
{
    public int IdRol { get; set; }

    public string Accion { get; set; } = null!;

    public virtual Role IdRolNavigation { get; set; } = null!;
}
