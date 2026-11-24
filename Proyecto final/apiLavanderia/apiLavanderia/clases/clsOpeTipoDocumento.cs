using apiLavanderia.Models;

namespace apiLavanderia.clases
{
    public class clsOpeTipoDocumento
    {
        private readonly LavanderiaContext ctx;

        public clsOpeTipoDocumento(LavanderiaContext ctx)
        {
            this.ctx = ctx;
        }

        //Propiedad de entrada y salida con los datos del tipo de documento
        public TblTipoDocumento tblTipoDocumento { get; set; }

        //Combo: tipos de documento
        public List<TblTipoDocumento> listaTipoDocumentos()
        {
            return ctx.TblTipoDocumentos
                .OrderBy(x => x.NombreDocumento)
                .ToList();
        }

        //iqueryable: consultar un tipo de documento por su nombre
        public IQueryable tipoDocumentoXNombre(string nombre)
        {
            return from ITD in ctx.Set<TblTipoDocumento>()
                   where ITD.NombreDocumento == nombre
                   select new {
                       codigo = ITD.IdTipoDocumento,
                       nombre = ITD.NombreDocumento
                   };
        }

        //iqueryable: listar todos los tipos de documento para llenar la tabla
        public IQueryable listarTipoDocumentosTabla()
        {
            return from ITD in ctx.Set<TblTipoDocumento>()
                   orderby ITD.NombreDocumento
                   select new {
                       editar = "<a class='btn btn-info btn-sm' href=''><i class='fa fa-search-plus'></i> ir </a>",
                       codigo = ITD.IdTipoDocumento,
                       nombre = ITD.NombreDocumento
                   }; // Los 3 <th></th> para la tabla
        }

        //Agregar: retorna 0 = error de ejecucion, -1 = ya existe el registro,
        //o el codigo (clave primaria) del registro ingresado
        public int Agregar()
        {
            try
            {
                // Validar si existe
                var existe = ctx.TblTipoDocumentos.FirstOrDefault(x => x.NombreDocumento == tblTipoDocumento.NombreDocumento);
                if (existe != null)
                {
                    return -1; // Ya existe
                }
                // Hallar el codigo
                int newCod = ctx.TblTipoDocumentos.Any() ? ctx.TblTipoDocumentos.Max(x => x.IdTipoDocumento) : 0;
                tblTipoDocumento.IdTipoDocumento = (newCod <= 0) ? 1 : newCod + 1;
                ctx.TblTipoDocumentos.Add(tblTipoDocumento);
                ctx.SaveChanges();
                return tblTipoDocumento.IdTipoDocumento;
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
                var rgro = ctx.TblTipoDocumentos.FirstOrDefault(x => x.IdTipoDocumento == tblTipoDocumento.IdTipoDocumento);
                if (rgro == null)
                {
                    return -1; // Error, no existe
                }
                // Actualiza cada dato
                rgro.NombreDocumento = tblTipoDocumento.NombreDocumento;
                ctx.SaveChanges();
                return tblTipoDocumento.IdTipoDocumento;
            }
            catch
            {
                return 0; // Error de ejecucion
            }
        }
    }
}
