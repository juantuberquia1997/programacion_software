using System;
using System.Collections.Generic;

namespace apiFestRock.Models;

public partial class TblEvento
{
    public int Codigo { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdCiudad { get; set; }

    public int Anio { get; set; }

    public string Fecha { get; set; } = null!;

    public string Lugar { get; set; } = null!;

    public decimal? VrInscEvent { get; set; }

    public bool Activo { get; set; }

    /*public virtual TblCiudad IdCiudadNavigation { get; set; } = null!;

    public virtual ICollection<TblInscrip> TblInscrips { get; set; } = new List<TblInscrip>();*/
}
