using System;
using System.Collections.Generic;

namespace apiFestRock.Models;

public partial class TblDpto
{
    public int Codigo { get; set; }

    public string Nombre { get; set; } = null!;

    /*public virtual ICollection<TblCiudad> TblCiudads { get; set; } = new List<TblCiudad>();*/
}
