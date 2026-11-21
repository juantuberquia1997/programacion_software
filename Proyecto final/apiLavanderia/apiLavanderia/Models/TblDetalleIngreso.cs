using System;
using System.Collections.Generic;

namespace apiLavanderia.Models;

public partial class TblDetalleIngreso
{
    public int IdDetalleIngreso { get; set; }

    public int IdDescuento { get; set; }

    public int IdTipoPrendaServicio { get; set; }

    public int IdCliente { get; set; }

    public int IdIngreso { get; set; }

    public int IdEstadoPrenda { get; set; }

    public int Cantidad { get; set; }

    public decimal Valor { get; set; }

    public string? Observacion { get; set; }
}
