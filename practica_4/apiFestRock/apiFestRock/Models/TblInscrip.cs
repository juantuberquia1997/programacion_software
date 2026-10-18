using System;
using System.Collections.Generic;

namespace apiFestRock.Models;

public partial class TblInscrip
{
    public int Codigo { get; set; }

    public int IdEvento { get; set; }

    public string? Fecha { get; set; }

    public int IdBanda { get; set; }

    public string Represent { get; set; } = null!;

    public string TelRepresent { get; set; } = null!;

    public decimal? ValorInscrip { get; set; }

   /* public virtual TblBandum IdBandaNavigation { get; set; } = null!;

    public virtual TblEvento IdEventoNavigation { get; set; } = null!;

    public virtual ICollection<TblDetInsc> TblDetInscs { get; set; } = new List<TblDetInsc>();*/
}
