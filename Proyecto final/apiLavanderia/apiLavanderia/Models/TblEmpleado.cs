using System;
using System.Collections.Generic;

namespace apiLavanderia.Models;

public partial class TblEmpleado
{
    public int IdEmpleado { get; set; }

    public int? EmpleadoIdEmpleado { get; set; }

    public int IdTipoDocumento { get; set; }

    public string NombreApellidoEmp { get; set; } = null!;

    public string DocumentoEmpleado { get; set; } = null!;

    public bool Activo { get; set; }

    public DateTime FechaIngreso { get; set; }

    public decimal Salario { get; set; }
}
