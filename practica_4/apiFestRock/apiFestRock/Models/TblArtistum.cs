using System;
using System.Collections.Generic;

namespace apiFestRock.Models;

public partial class TblArtistum
{
    public int Codigo { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdtipoDoc { get; set; }

    public int NroDoc { get; set; }

    public int IdGenero { get; set; }

    public int IdCiudad { get; set; }

    public bool Activo { get; set; }

    /*public virtual TblCiudad IdCiudadNavigation { get; set; } = null!;

    public virtual TblGenero IdGeneroNavigation { get; set; } = null!;

    public virtual TblTipoDoc IdtipoDocNavigation { get; set; } = null!;

    public virtual ICollection<TblArtBand> TblArtBands { get; set; } = new List<TblArtBand>();*/
}
