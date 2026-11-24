using apiLavanderia.Models;

namespace apiLavanderia.clases
{
    public class clsOpeTipoServicio
    {
        private readonly LavanderiaContext ctx;

        public clsOpeTipoServicio(LavanderiaContext ctx)
        {
            this.ctx = ctx;
        }

        //Propiedad de entrada y salida con los datos del tipo de servicio
        public TblTipoServicio tblTipoServicio { get; set; }

        //iqueryable: consultar un tipo de servicio por su nombre
        public IQueryable tipoServicioXNombre(string nombre)
        {
            return from IS in ctx.Set<TblTipoServicio>()
                   where IS.NombreServicio == nombre
                   select new {
                       codigo = IS.IdTipoServicio,
                       nombre = IS.NombreServicio,
                       valor  = IS.Valor,
                       act    = IS.Activo
                   };
        }

        //iqueryable: listar todos los tipos de servicio para llenar la tabla
        public IQueryable listarTipoServiciosTabla()
        {
            return from IS in ctx.Set<TblTipoServicio>()
                   orderby IS.NombreServicio
                   select new {
                       editar = "<a class='btn btn-info btn-sm' href=''><i class='fa fa-search-plus'></i> ir </a>",
                       codigo = IS.IdTipoServicio,
                       nombre = IS.NombreServicio,
                       valor  = IS.Valor,
                       activo = IS.Activo
                   }; // Los 5 <th></th> para la tabla
        }

        //Agregar: retorna 0 = error de ejecucion, -1 = ya existe el registro,
        //o el codigo (clave primaria) del registro ingresado
        public int Agregar()
        {
            try
            {
                // Validar si existe
                var existe = ctx.TblTipoServicios.FirstOrDefault(x => x.NombreServicio == tblTipoServicio.NombreServicio);
                if (existe != null)
                {
                    return -1; // Ya existe
                }
                // Hallar el codigo
                int newCod = ctx.TblTipoServicios.Any() ? ctx.TblTipoServicios.Max(x => x.IdTipoServicio) : 0;
                tblTipoServicio.IdTipoServicio = (newCod <= 0) ? 1 : newCod + 1;
                ctx.TblTipoServicios.Add(tblTipoServicio);
                ctx.SaveChanges();
                return tblTipoServicio.IdTipoServicio;
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
                var rgro = ctx.TblTipoServicios.FirstOrDefault(x => x.IdTipoServicio == tblTipoServicio.IdTipoServicio);
                if (rgro == null)
                {
                    return -1; // Error, no existe
                }
                // Actualiza cada dato
                rgro.NombreServicio = tblTipoServicio.NombreServicio;
                rgro.Valor = tblTipoServicio.Valor;
                rgro.Activo = tblTipoServicio.Activo;
                ctx.SaveChanges();
                return tblTipoServicio.IdTipoServicio;
            }
            catch
            {
                return 0; // Error de ejecucion
            }
        }
    }
}
