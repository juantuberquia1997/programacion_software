using System;
using System.Collections.Generic;

namespace apiLavanderia.Models;

public partial class TblIngreso
{
    public int IdIngreso { get; set; }

    public int IdCliente { get; set; }

    public int IdDireccion { get; set; }

    public int EmpleadoIdEmpleado { get; set; }

    public DateTime Fecha { get; set; }

    public bool Domicilio { get; set; }

    public bool Activo { get; set; }
}
