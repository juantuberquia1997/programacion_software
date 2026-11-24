using System;
using System.Collections.Generic;

namespace apiLavanderia.Models;

public partial class TblCiudad
{
    public int IdCiudad { get; set; }

    public int IdDepartamento { get; set; }

    public string NombreCiudad { get; set; } = null!;
}
