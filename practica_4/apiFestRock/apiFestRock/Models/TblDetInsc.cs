using System;
using System.Collections.Generic;

namespace apiFestRock.Models;

public partial class TblDetInsc
{
    public int Codigo { get; set; }

    public int IdNroInsc { get; set; }

    public int IdArtBand { get; set; }

    public int Edad { get; set; }

    /*public virtual TblArtBand IdArtBandNavigation { get; set; } = null!;

    public virtual TblInscrip IdNroInscNavigation { get; set; } = null!;*/
}
