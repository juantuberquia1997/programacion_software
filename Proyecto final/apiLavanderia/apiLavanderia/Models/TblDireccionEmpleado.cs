using System;
using System.Collections.Generic;

namespace apiLavanderia.Models;

public partial class TblDireccionEmpleado
{
    public int IdDireccionEmpleado { get; set; }

    public int IdCiudad { get; set; }

    public int EmpleadoIdEmpleado { get; set; }

    public string DireccionEmpleado { get; set; } = null!;
}
