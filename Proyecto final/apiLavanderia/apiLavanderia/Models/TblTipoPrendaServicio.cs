using System;
using System.Collections.Generic;

namespace apiLavanderia.Models;

public partial class TblTipoPrendaServicio
{
    public int IdTipoPrendaServicio { get; set; }

    public int IdTipoServicio { get; set; }

    public int IdTipoPrenda { get; set; }

    public decimal ValorUnitario { get; set; }

    public bool Activo { get; set; }
}
