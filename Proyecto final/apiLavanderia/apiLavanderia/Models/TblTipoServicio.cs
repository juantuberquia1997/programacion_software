using System;
using System.Collections.Generic;

namespace apiLavanderia.Models;

public partial class TblTipoServicio
{
    public int IdTipoServicio { get; set; }

    public string NombreServicio { get; set; } = null!;

    public int Valor { get; set; }

    public bool Activo { get; set; }
}
