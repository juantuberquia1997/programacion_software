using System;
using System.Collections.Generic;

namespace apiFestRock.Models;

public partial class TblBandum
{
    public int Codigo { get; set; }

    public string Nombre { get; set; } = null!;

    public int AnioFund { get; set; }

    public string Represent { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public int IdCiudad { get; set; }

    public bool Activo { get; set; }

    /*public virtual TblCiudad IdCiudadNavigation { get; set; } = null!;

    public virtual ICollection<TblArtBand> TblArtBands { get; set; } = new List<TblArtBand>();

    public virtual ICollection<TblInscrip> TblInscrips { get; set; } = new List<TblInscrip>();*/
}
