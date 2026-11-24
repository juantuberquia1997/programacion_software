using System;
using System.Collections.Generic;

namespace apiLavanderia.Models;

public partial class TblEmpleadoHasProfesion
{
    public int EmpleadoIdEmpleado { get; set; }

    public int ProfesionIdProfesion { get; set; }
}
