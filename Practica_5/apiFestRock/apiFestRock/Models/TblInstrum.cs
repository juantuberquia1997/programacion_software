using System;
using System.Collections.Generic;

namespace apiFestRock.Models;

public partial class TblInstrum
{
    public int Codigo { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Activo { get; set; }

    /*public virtual ICollection<TblArtBand> TblArtBands { get; set; } = new List<TblArtBand>();*/
}
