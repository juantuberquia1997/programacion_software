using System;
using System.Collections.Generic;

namespace apiFestRock.Models;

public partial class TblArtBand
{
    public int Codigo { get; set; }

    public int IdBanda { get; set; }

    public int IdArtista { get; set; }

    public int IdInstrum { get; set; }

    public bool Activo { get; set; }

    /*public virtual TblArtistum IdArtistaNavigation { get; set; } = null!;

    public virtual TblBandum IdBandaNavigation { get; set; } = null!;

    public virtual TblInstrum IdInstrumNavigation { get; set; } = null!;

    public virtual ICollection<TblDetInsc> TblDetInscs { get; set; } = new List<TblDetInsc>();*/
}
