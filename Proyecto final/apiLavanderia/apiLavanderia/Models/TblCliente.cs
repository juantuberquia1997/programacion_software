using System;
using System.Collections.Generic;

namespace apiLavanderia.Models;

public partial class TblCliente
{
    public int IdCliente { get; set; }

    public int EmpleadoIdEmpleado { get; set; }

    public int IdTipoDocumento { get; set; }

    public string NombreApellidoCliente { get; set; } = null!;

    public string Documento { get; set; } = null!;

    public long NumeroDocumento { get; set; }
}
