using System;
using System.Collections.Generic;

namespace apiLavanderia.Models;

public partial class TblUsuario
{
    public int IdUsuarios { get; set; }

    public int EmpleadoIdEmpleado { get; set; }

    public string Usuario { get; set; } = null!;

    public string Clave { get; set; } = null!;

    public bool Activo { get; set; }
}
