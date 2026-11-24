using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace apiLavanderia.Models;

public partial class TblCliente
{
    public int IdCliente { get; set; }

    public int EmpleadoIdEmpleado { get; set; }

    public int IdTipoDocumento { get; set; }

    public string NombreApellidoCliente { get; set; } = null!;

    [Column("documento")]
    public string NumeroDocumento { get; set; } = null!;
}
