using apiLavanderia.Models;

namespace apiLavanderia.clases
{
    public class clsOpeTipoPrendaServicio
    {
        private readonly LavanderiaContext ctx;

        public clsOpeTipoPrendaServicio(LavanderiaContext ctx)
        {
            this.ctx = ctx;
        }

        //Propiedad de entrada y salida con los datos de la tarifa prenda-servicio
        public TblTipoPrendaServicio tblTipoPrendaServicio { get; set; }

        //iqueryable: combo de tarifas activas con descripcion (servicio - prenda)
        public IQueryable comboTipoPrendaServicios()
        {
            return from PS in ctx.Set<TblTipoPrendaServicio>()
                   join IS in ctx.Set<TblTipoServicio>() on PS.IdTipoServicio equals IS.IdTipoServicio
                   join IP in ctx.Set<TblTipoPrenda>() on PS.IdTipoPrenda equals IP.IdTipoPrenda
                   where PS.Activo == true
                   orderby IS.NombreServicio, IP.NombrePrenda
                   select new {
                       codigo        = PS.IdTipoPrendaServicio,
                       nombre        = IS.NombreServicio + " - " + IP.NombrePrenda,
                       idPrenda      = PS.IdTipoPrenda,
                       valorUnitario = PS.ValorUnitario
                   };
        }

        //Agregar: retorna 0 = error de ejecucion, -1 = ya existe el registro,
        //o el codigo (clave primaria) del registro ingresado
        public int Agregar()
        {
            try
            {
                // Validar si existe
                var existe = ctx.TblTipoPrendaServicios.FirstOrDefault(x => x.IdTipoServicio == tblTipoPrendaServicio.IdTipoServicio && x.IdTipoPrenda == tblTipoPrendaServicio.IdTipoPrenda);
                if (existe != null)
                {
                    return -1; // Ya existe
                }
                // Hallar el codigo
                int newCod = ctx.TblTipoPrendaServicios.Any() ? ctx.TblTipoPrendaServicios.Max(x => x.IdTipoPrendaServicio) : 0;
                tblTipoPrendaServicio.IdTipoPrendaServicio = (newCod <= 0) ? 1 : newCod + 1;
                ctx.TblTipoPrendaServicios.Add(tblTipoPrendaServicio);
                ctx.SaveChanges();
                return tblTipoPrendaServicio.IdTipoPrendaServicio;
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
                var rgro = ctx.TblTipoPrendaServicios.FirstOrDefault(x => x.IdTipoPrendaServicio == tblTipoPrendaServicio.IdTipoPrendaServicio);
                if (rgro == null)
                {
                    return -1; // Error, no existe
                }
                // Actualiza cada dato
                rgro.IdTipoServicio = tblTipoPrendaServicio.IdTipoServicio;
                rgro.IdTipoPrenda = tblTipoPrendaServicio.IdTipoPrenda;
                rgro.ValorUnitario = tblTipoPrendaServicio.ValorUnitario;
                rgro.Activo = tblTipoPrendaServicio.Activo;
                ctx.SaveChanges();
                return tblTipoPrendaServicio.IdTipoPrendaServicio;
            }
            catch
            {
                return 0; // Error de ejecucion
            }
        }
    }
}
