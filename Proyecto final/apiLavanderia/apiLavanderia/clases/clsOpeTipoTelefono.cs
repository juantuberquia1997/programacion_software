using apiLavanderia.Models;

namespace apiLavanderia.clases
{
    public class clsOpeTipoTelefono
    {
        private readonly LavanderiaContext ctx;

        public clsOpeTipoTelefono(LavanderiaContext ctx)
        {
            this.ctx = ctx;
        }

        //Propiedad de entrada y salida con los datos del tipo de telefono
        public TblTipoTelefono tblTipoTelefono { get; set; }

        //Combo: tipos de telefono
        public List<TblTipoTelefono> listaTipoTelefonos()
        {
            return ctx.TblTipoTelefonos
                .OrderBy(x => x.TipoTelefono)
                .ToList();
        }

        //iqueryable: consultar un tipo de telefono por su nombre
        public IQueryable tipoTelefonoXNombre(string nombre)
        {
            return from IT in ctx.Set<TblTipoTelefono>()
                   where IT.TipoTelefono == nombre
                   select new {
                       codigo = IT.IdTipoTelefono,
                       nombre = IT.TipoTelefono
                   };
        }

        //iqueryable: listar todos los tipos de telefono para llenar la tabla
        public IQueryable listarTipoTelefonosTabla()
        {
            return from IT in ctx.Set<TblTipoTelefono>()
                   orderby IT.TipoTelefono
                   select new {
                       editar = "<a class='btn btn-info btn-sm' href=''><i class='fa fa-search-plus'></i> ir </a>",
                       codigo = IT.IdTipoTelefono,
                       nombre = IT.TipoTelefono
                   }; // Los 3 <th></th> para la tabla
        }

        //Agregar: retorna 0 = error de ejecucion, -1 = ya existe el registro,
        //o el codigo (clave primaria) del registro ingresado
        public int Agregar()
        {
            try
            {
                // Validar si existe
                var existe = ctx.TblTipoTelefonos.FirstOrDefault(x => x.TipoTelefono == tblTipoTelefono.TipoTelefono);
                if (existe != null)
                {
                    return -1; // Ya existe
                }
                // Hallar el codigo
                int newCod = ctx.TblTipoTelefonos.Any() ? ctx.TblTipoTelefonos.Max(x => x.IdTipoTelefono) : 0;
                tblTipoTelefono.IdTipoTelefono = (newCod <= 0) ? 1 : newCod + 1;
                ctx.TblTipoTelefonos.Add(tblTipoTelefono);
                ctx.SaveChanges();
                return tblTipoTelefono.IdTipoTelefono;
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
                var rgro = ctx.TblTipoTelefonos.FirstOrDefault(x => x.IdTipoTelefono == tblTipoTelefono.IdTipoTelefono);
                if (rgro == null)
                {
                    return -1; // Error, no existe
                }
                // Actualiza cada dato
                rgro.TipoTelefono = tblTipoTelefono.TipoTelefono;
                ctx.SaveChanges();
                return tblTipoTelefono.IdTipoTelefono;
            }
            catch
            {
                return 0; // Error de ejecucion
            }
        }
    }
}
