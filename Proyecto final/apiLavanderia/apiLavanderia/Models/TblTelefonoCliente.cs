using System;
using System.Collections.Generic;

namespace apiLavanderia.Models;

public partial class TblTelefonoCliente
{
    public int IdTelefono { get; set; }

    public int IdTipoTelefono { get; set; }

    public int IdCliente { get; set; }

    public string Numero { get; set; } = null!;
}
