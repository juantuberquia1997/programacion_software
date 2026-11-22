using System;
using System.Collections.Generic;

namespace apiLavanderia.Models;

public partial class TblTipoPrenda
{
    public int IdTipoPrenda { get; set; }

    public string NombrePrenda { get; set; } = null!;
    
    public bool Activo { get; set; }
}
