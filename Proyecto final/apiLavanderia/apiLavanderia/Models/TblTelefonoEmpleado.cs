using System;
using System.Collections.Generic;

namespace apiLavanderia.Models;

public partial class TblTelefonoEmpleado
{
    public int IdTelefonoEmpleado { get; set; }

    public int IdTipoTelefono { get; set; }

    public int EmpleadoIdEmpleado { get; set; }

    public string Numero { get; set; } = null!;
}
