using apiLavanderia.Models;
using static apiLavanderia.clases.auxiliar.clsIngresoCompleto;

namespace apiLavanderia.clases
{
    public class clsOpeIngreso
    {
        private readonly LavanderiaContext ctx;

        public clsOpeIngreso(LavanderiaContext ctx)
        {
            this.ctx = ctx;
        }

        //Propiedad con los datos del encabezado del ingreso
        public TblIngreso tblIngreso { get; set; }

        //iqueryable: consultar el encabezado de un ingreso por su codigo
        public IQueryable consultarIngreso(int idIngreso)
        {
            return from IN in ctx.Set<TblIngreso>()
                   join IC in ctx.Set<TblCliente>() on IN.IdCliente equals IC.IdCliente
                   join IE in ctx.Set<TblEmpleado>() on IN.EmpleadoIdEmpleado equals IE.IdEmpleado
                   where IN.IdIngreso == idIngreso
                   select new {
                       idIngreso = IN.IdIngreso,
                       idCliente = IN.IdCliente,
                       cliente   = IC.NombreApellidoCliente,
                       idDir     = IN.IdDireccion,
                       idEmp     = IN.EmpleadoIdEmpleado,
                       empleado  = IE.NombreApellidoEmp,
                       fecha     = IN.Fecha,
                       domicilio = IN.Domicilio,
                       activo    = IN.Activo
                   };
        }

        //iqueryable: listar todos los ingresos para llenar la tabla
        public IQueryable listarIngresosTabla()
        {
            return from IN in ctx.Set<TblIngreso>()
                   join IC in ctx.Set<TblCliente>() on IN.IdCliente equals IC.IdCliente
                   join IE in ctx.Set<TblEmpleado>() on IN.EmpleadoIdEmpleado equals IE.IdEmpleado
                   orderby IN.IdIngreso
                   select new {
                       editar   = "<a class='btn btn-info btn-sm' href=''><i class='fa fa-search-plus'></i> ir </a>",
                       idIngreso = IN.IdIngreso,
                       cliente   = IC.NombreApellidoCliente,
                       empleado  = IE.NombreApellidoEmp,
                       fecha     = IN.Fecha,
                       domicilio = IN.Domicilio,
                       activo    = IN.Activo
                   }; // Los 7 <th></th> para la tabla
        }

        //iqueryable: detalle de un ingreso (prendas a lavar con su estado)
        public IQueryable detalleIngreso(int idIngreso)
        {
            return from DI in ctx.Set<TblDetalleIngreso>()
                   join PS in ctx.Set<TblTipoPrendaServicio>() on DI.IdTipoPrendaServicio equals PS.IdTipoPrendaServicio
                   join IS in ctx.Set<TblTipoServicio>() on PS.IdTipoServicio equals IS.IdTipoServicio
                   join IP in ctx.Set<TblTipoPrenda>() on PS.IdTipoPrenda equals IP.IdTipoPrenda
                   join EP in ctx.Set<TblEstadoPrenda>() on DI.IdEstadoPrenda equals EP.IdEstadoPrenda
                   where DI.IdIngreso == idIngreso
                   orderby DI.IdDetalleIngreso
                   select new {
                       idDet     = DI.IdDetalleIngreso,
                       servicio  = IS.NombreServicio,
                       prenda    = IP.NombrePrenda,
                       estado    = EP.EstadoPrenda,
                       cantidad  = DI.Cantidad,
                       valor     = DI.Valor,
                       observacion = DI.Observacion
                   };
        }

        //Grabar el encabezado del ingreso: retorna el codigo generado para el
        //nuevo registro
        public int registrarEncabezado(ingresoCompleta datosEncab)
        {
            var ultimo = ctx.TblIngresos.Max(x => (int?)x.IdIngreso) ?? 0;
            int nuevoID = (ultimo <= 0) ? 1 : ultimo + 1;

            TblIngreso tblIngreso = new TblIngreso();
            tblIngreso.IdIngreso = nuevoID;
            tblIngreso.IdCliente = datosEncab.IdCliente;
            tblIngreso.IdDireccion = datosEncab.IdDireccion;
            tblIngreso.EmpleadoIdEmpleado = datosEncab.EmpleadoIdEmpleado;
            tblIngreso.Fecha = datosEncab.Fecha;
            tblIngreso.Domicilio = datosEncab.Domicilio;
            tblIngreso.Activo = datosEncab.Activo;

            ctx.TblIngresos.Add(tblIngreso);
            ctx.SaveChanges();
            return nuevoID;
        }

        //Grabar el detalle (prendas) de un ingreso: retorna true si grabo todos
        //los registros, false en caso de error. Calcula y aplica automaticamente
        //el descuento por cantidad de prendas (RF07)
        public bool registrarDetalles(int idIngreso, int idCliente, List<detalleIngreso> prendas)
        {
            try
            {
                int maxDet = ctx.TblDetalleIngresos.Any() ? ctx.TblDetalleIngresos.Max(x => x.IdDetalleIngreso) : 0;
                foreach (var pren in prendas)
                {
                    // Tarifa de la prenda (servicio + prenda -> valor unitario)
                    var tps = ctx.TblTipoPrendaServicios.FirstOrDefault(x => x.IdTipoPrendaServicio == pren.IdTipoPrendaServicio);
                    if (tps == null)
                    {
                        return false;
                    }
                    // Descuento aplicable segun la cantidad de prendas (RF07)
                    var desc = ctx.TblDescuentos.FirstOrDefault(x => x.IdTipoPrenda == tps.IdTipoPrenda
                                                                   && x.Activo == true
                                                                   && pren.Cantidad >= x.CantidadMin
                                                                   && pren.Cantidad <= x.CantidadMax);
                    decimal valorBase = tps.ValorUnitario * pren.Cantidad;
                    decimal valorFinal = valorBase;
                    // Referencia del descuento de la prenda (la FK no admite null)
                    var descRef = desc ?? ctx.TblDescuentos.FirstOrDefault(x => x.IdTipoPrenda == tps.IdTipoPrenda);
                    int idDesc = descRef != null ? descRef.IdDescuento : 0;
                    if (desc != null)
                    {
                        valorFinal = valorBase * (1 - (desc.Porcentaje / 100m));
                    }

                    maxDet = maxDet + 1;
                    TblDetalleIngreso tblDet = new TblDetalleIngreso();
                    tblDet.IdDetalleIngreso = maxDet;
                    tblDet.IdIngreso = idIngreso;
                    tblDet.IdCliente = idCliente;
                    tblDet.IdTipoPrendaServicio = pren.IdTipoPrendaServicio;
                    tblDet.IdDescuento = idDesc;
                    tblDet.IdEstadoPrenda = pren.IdEstadoPrenda;
                    tblDet.Cantidad = pren.Cantidad;
                    tblDet.Valor = valorFinal;
                    tblDet.Observacion = pren.Observacion;
                    ctx.TblDetalleIngresos.Add(tblDet);
                }
                ctx.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Eliminar el encabezado del ingreso (rollback manual cuando falla el
        //grabado del detalle)
        public void eliminarEncabezado(int idCod)
        {
            try
            {
                var rgtro = ctx.TblIngresos.FirstOrDefault(d => d.IdIngreso == idCod);
                if (rgtro != null)
                {
                    ctx.TblIngresos.Remove(rgtro);
                    ctx.SaveChanges();
                }
            }
            catch
            {
                return;
            }
        }

        //Modificar el encabezado del ingreso usando la propiedad tblIngreso
        public string Modificar()
        {
            try
            {
                if (tblIngreso.IdIngreso <= 0)
                {
                    return "Ingreso: " + tblIngreso.IdIngreso + ", no es valido.";
                }

                var p = ctx.TblIngresos.FirstOrDefault(x => x.IdIngreso == tblIngreso.IdIngreso);

                if (p == null)
                {
                    return "Ingreso " + tblIngreso.IdIngreso + ", no existe.";
                }

                p.IdCliente = tblIngreso.IdCliente;
                p.IdDireccion = tblIngreso.IdDireccion;
                p.EmpleadoIdEmpleado = tblIngreso.EmpleadoIdEmpleado;
                p.Domicilio = tblIngreso.Domicilio;
                p.Activo = tblIngreso.Activo;

                ctx.SaveChanges();
                return "Ingreso: " + tblIngreso.IdIngreso + ", actualizado correctamente.";
            }
            catch
            {
                return "Error al actualizar ingreso.";
            }
        }
    }
}
