using System;
using System.Collections.Generic;

namespace apiLavanderia.Models;

public partial class TblDescuento
{
    public int IdDescuento { get; set; }

    public int IdTipoPrenda { get; set; }

    public int CantidadMin { get; set; }

    public int CantidadMax { get; set; }

    public decimal Porcentaje { get; set; }

    public bool Activo { get; set; }
}
