using System;
using System.Collections.Generic;

namespace apiFestRock.Models;

public partial class TblGenero
{
    public int Codigo { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Activo { get; set; }

    /*public virtual ICollection<TblArtistum> TblArtista { get; set; } = new List<TblArtistum>();*/
}
