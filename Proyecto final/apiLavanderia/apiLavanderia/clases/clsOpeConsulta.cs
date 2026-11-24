using apiLavanderia.Models;

namespace apiLavanderia.clases
{
    //Consultas por cliente, empleado y estado de prenda (form Consultas)
    public class clsOpeConsulta
    {
        private readonly LavanderiaContext ctx;

        public clsOpeConsulta(LavanderiaContext ctx)
        {
            this.ctx = ctx;
        }

        //iqueryable: ingresos de un cliente
        public IQueryable ingresosXCliente(int idCliente)
        {
            return from IN in ctx.Set<TblIngreso>()
                   join IC in ctx.Set<TblCliente>() on IN.IdCliente equals IC.IdCliente
                   join IE in ctx.Set<TblEmpleado>() on IN.EmpleadoIdEmpleado equals IE.IdEmpleado
                   where IN.IdCliente == idCliente
                   orderby IN.Fecha
                   select new {
                       idIngreso = IN.IdIngreso,
                       cliente   = IC.NombreApellidoCliente,
                       empleado  = IE.NombreApellidoEmp,
                       fecha     = IN.Fecha,
                       domicilio = IN.Domicilio,
                       total     = ctx.Set<TblDetalleIngreso>().Where(D => D.IdIngreso == IN.IdIngreso).Sum(S => (decimal?)S.Valor) ?? 0
                   };
        }

        //iqueryable: ingresos realizados por un empleado
        public IQueryable ingresosXEmpleado(int idEmpleado)
        {
            return from IN in ctx.Set<TblIngreso>()
                   join IC in ctx.Set<TblCliente>() on IN.IdCliente equals IC.IdCliente
                   join IE in ctx.Set<TblEmpleado>() on IN.EmpleadoIdEmpleado equals IE.IdEmpleado
                   where IN.EmpleadoIdEmpleado == idEmpleado
                   orderby IN.Fecha
                   select new {
                       idIngreso = IN.IdIngreso,
                       cliente   = IC.NombreApellidoCliente,
                       empleado  = IE.NombreApellidoEmp,
                       fecha     = IN.Fecha,
                       domicilio = IN.Domicilio
                   };
        }

        //iqueryable: prendas de un estado (recibida, en lavado, entregada, etc.)
        public IQueryable prendasXEstado(int idEstado)
        {
            return from DI in ctx.Set<TblDetalleIngreso>()
                   join PS in ctx.Set<TblTipoPrendaServicio>() on DI.IdTipoPrendaServicio equals PS.IdTipoPrendaServicio
                   join IS in ctx.Set<TblTipoServicio>() on PS.IdTipoServicio equals IS.IdTipoServicio
                   join IP in ctx.Set<TblTipoPrenda>() on PS.IdTipoPrenda equals IP.IdTipoPrenda
                   join EP in ctx.Set<TblEstadoPrenda>() on DI.IdEstadoPrenda equals EP.IdEstadoPrenda
                   join IN in ctx.Set<TblIngreso>() on DI.IdIngreso equals IN.IdIngreso
                   join IC in ctx.Set<TblCliente>() on DI.IdCliente equals IC.IdCliente
                   where DI.IdEstadoPrenda == idEstado
                   orderby IN.IdIngreso
                   select new {
                       idIngreso = DI.IdIngreso,
                       cliente   = IC.NombreApellidoCliente,
                       servicio  = IS.NombreServicio,
                       prenda    = IP.NombrePrenda,
                       estado    = EP.EstadoPrenda,
                       cantidad  = DI.Cantidad,
                       valor     = DI.Valor
                   };
        }
    }
}
