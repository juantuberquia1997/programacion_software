using apiLavanderia.Models;

namespace apiLavanderia.clases
{
    public class clsOpeDescuento
    {
        private readonly LavanderiaContext ctx;

        public clsOpeDescuento(LavanderiaContext ctx)
        {
            this.ctx = ctx;
        }

        //Propiedad de entrada y salida con los datos del descuento
        public TblDescuento tblDescuento { get; set; }

        //iqueryable: consultar el descuento por tipo de prenda
        public IQueryable descuentoXPrenda(int idPrenda)
        {
            return from ID in ctx.Set<TblDescuento>()
                   where ID.IdTipoPrenda == idPrenda && ID.Activo == true
                   select new {
                       codigo    = ID.IdDescuento,
                       idPrenda  = ID.IdTipoPrenda,
                       cantMin   = ID.CantidadMin,
                       cantMax   = ID.CantidadMax,
                       porcen    = ID.Porcentaje
                   };
        }

        //iqueryable: listar todos los descuentos para llenar la tabla
        public IQueryable listarDescuentosTabla()
        {
            return from ID in ctx.Set<TblDescuento>()
                   join IP in ctx.Set<TblTipoPrenda>() on ID.IdTipoPrenda equals IP.IdTipoPrenda
                   orderby IP.NombrePrenda, ID.CantidadMin
                   select new {
                       editar    = "<a class='btn btn-info btn-sm' href=''><i class='fa fa-search-plus'></i> ir </a>",
                       codigo    = ID.IdDescuento,
                       prenda    = IP.NombrePrenda,
                       cantMin   = ID.CantidadMin,
                       cantMax   = ID.CantidadMax,
                       porcen    = ID.Porcentaje,
                       activo    = ID.Activo
                   }; // Los 7 <th></th> para la tabla
        }

        //Agregar: retorna 0 = error de ejecucion, -1 = ya existe el registro,
        //o el codigo (clave primaria) del registro ingresado
        public int Agregar()
        {
            try
            {
                // Validar si existe
                var existe = ctx.TblDescuentos.FirstOrDefault(x => x.IdTipoPrenda == tblDescuento.IdTipoPrenda);
                if (existe != null)
                {
                    return -1; // Ya existe
                }
                // Hallar el codigo
                int newCod = ctx.TblDescuentos.Any() ? ctx.TblDescuentos.Max(x => x.IdDescuento) : 0;
                tblDescuento.IdDescuento = (newCod <= 0) ? 1 : newCod + 1;
                ctx.TblDescuentos.Add(tblDescuento);
                ctx.SaveChanges();
                return tblDescuento.IdDescuento;
            }
            catch
            {
                return 0; // Error de ejecucion
            }
        }

        //Modificar: retorna 0 = error de ejecucion, -1 = no existe el registro,
        //o el codigo (clave primaria) del registro modificado
        public int Modificar()
        {
            try
            {
                // Validar si existe
                var rgro = ctx.TblDescuentos.FirstOrDefault(x => x.IdDescuento == tblDescuento.IdDescuento);
                if (rgro == null)
                {
                    return -1; // Error, no existe
                }
                // Actualiza cada dato
                rgro.IdTipoPrenda = tblDescuento.IdTipoPrenda;
                rgro.CantidadMin = tblDescuento.CantidadMin;
                rgro.CantidadMax = tblDescuento.CantidadMax;
                rgro.Porcentaje = tblDescuento.Porcentaje;
                rgro.Activo = tblDescuento.Activo;
                ctx.SaveChanges();
                return tblDescuento.IdDescuento;
            }
            catch
            {
                return 0; // Error de ejecucion
            }
        }
    }
}
