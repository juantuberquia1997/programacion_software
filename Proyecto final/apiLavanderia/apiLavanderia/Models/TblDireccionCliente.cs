using System;
using System.Collections.Generic;

namespace apiLavanderia.Models;

public partial class TblDireccionCliente
{
    public int IdDireccion { get; set; }

    public int IdCiudad { get; set; }

    public int IdCliente { get; set; }

    public string DireccionCliente { get; set; } = null!;
}
