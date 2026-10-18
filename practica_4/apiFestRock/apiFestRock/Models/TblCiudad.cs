using System;
using System.Collections.Generic;

namespace apiFestRock.Models;

public partial class TblCiudad
{
    public int Codigo { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdDpto { get; set; }

    /*public virtual TblDpto IdDptoNavigation { get; set; } = null!;

    public virtual ICollection<TblArtistum> TblArtista { get; set; } = new List<TblArtistum>();

    public virtual ICollection<TblBandum> TblBanda { get; set; } = new List<TblBandum>();

    public virtual ICollection<TblEvento> TblEventos { get; set; } = new List<TblEvento>();*/
}
