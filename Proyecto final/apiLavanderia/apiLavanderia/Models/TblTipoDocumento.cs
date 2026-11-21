using System;
using System.Collections.Generic;

namespace apiLavanderia.Models;

public partial class TblTipoDocumento
{
    public int IdTipoDocumento { get; set; }

    public string NombreDocumento { get; set; } = null!;
}
