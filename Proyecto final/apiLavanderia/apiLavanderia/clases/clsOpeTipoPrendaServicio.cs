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

        //Combo: tarifas activas (servicio + prenda + valor unitario)
        public List<TblTipoPrendaServicio> listaTipoPrendaServicios()
        {
            return ctx.TblTipoPrendaServicios
                .Where(x => x.Activo == true)
                .OrderBy(x => x.IdTipoPrendaServicio)
                .ToList();
        }

        //iqueryable: consultar el valor unitario de un servicio para una prenda
        public IQueryable valorUnitarioXPrendaServicio(int idServicio, int idPrenda)
        {
            return from PS in ctx.Set<TblTipoPrendaServicio>()
                   where PS.IdTipoServicio == idServicio && PS.IdTipoPrenda == idPrenda && PS.Activo == true
                   select new {
                       codigo       = PS.IdTipoPrendaServicio,
                       valorUnit    = PS.ValorUnitario
                   };
        }

        //iqueryable: listar todas las tarifas para llenar la tabla
        public IQueryable listarTipoPrendaServiciosTabla()
        {
            return from PS in ctx.Set<TblTipoPrendaServicio>()
                   join IS in ctx.Set<TblTipoServicio>() on PS.IdTipoServicio equals IS.IdTipoServicio
                   join IP in ctx.Set<TblTipoPrenda>() on PS.IdTipoPrenda equals IP.IdTipoPrenda
                   orderby IS.NombreServicio, IP.NombrePrenda
                   select new {
                       editar        = "<a class='btn btn-info btn-sm' href=''><i class='fa fa-search-plus'></i> ir </a>",
                       codigo        = PS.IdTipoPrendaServicio,
                       servicio      = IS.NombreServicio,
                       prenda        = IP.NombrePrenda,
                       valorUnitario = PS.ValorUnitario,
                       activo        = PS.Activo
                   }; // Los 6 <th></th> para la tabla
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
