using apiFestRock.Models;

namespace apiFestRock.clases
{
    public class clsOpeTipoDoc
    {
        private readonly BdEventosContext ctx;

        public clsOpeTipoDoc(BdEventosContext ctx)
        {
            this.ctx = ctx;
        }

        //Propiedad de entrada y salida con los datos del tipo de documento
        public TblTipoDoc tblTipDoc { get; set; }

        //Combo: tipos de documento activos
        public List<TblTipoDoc> listaTipDocs()
        {
            return ctx.TblTipoDocs
                .Where(x => x.Activo == true)
                .OrderBy(x => x.Nombre)
                .ToList();
        }

        //iqueryable: consultar un tipo de documento por su nombre
        public IQueryable tipDocXNombre(string nombre)
        {
            return from IT in ctx.Set<TblTipoDoc>()
                   where IT.Nombre == nombre
                   select new {
                       codigo = IT.Codigo,
                       nombre = IT.Nombre,
                       act    = IT.Activo
                   };
        }

        //iqueryable: listar todos los tipos de documento para llenar la tabla
        public IQueryable listarTipDocsTabla()
        {
            return from IT in ctx.Set<TblTipoDoc>()
                   orderby IT.Nombre
                   select new {
                       editar = "<a class='btn btn-info btn-sm' href=''><i class='fa fa-search-plus'></i> ir </a>",
                       codigo = IT.Codigo,
                       nombre = IT.Nombre,
                       activo = IT.Activo
                   }; // Los 4 <th></th> para la tabla
        }

        //Agregar: retorna 0 = error de ejecución, -1 = ya existe el registro,
        //o el código (clave primaria) del registro ingresado
        public int Agregar()
        {
            try
            {
                // Validar si existe
                var existe = ctx.TblTipoDocs.FirstOrDefault(x => x.Nombre == tblTipDoc.Nombre);
                if (existe != null)
                {
                    return -1; // Ya existe
                }
                // Hallar el código
                int newCod = ctx.TblTipoDocs.Any() ? ctx.TblTipoDocs.Max(x => x.Codigo) : 0;
                tblTipDoc.Codigo = (newCod <= 0) ? 1 : newCod + 1;
                ctx.TblTipoDocs.Add(tblTipDoc);
                ctx.SaveChanges();
                return tblTipDoc.Codigo;
            }
            catch
            {
                return 0; // Error de ejecución
            }
        }

        //Modificar: retorna 0 = error de ejecución, -1 = no existe el registro,
        //o el código (clave primaria) del registro modificado
        public int Modificar()
        {
            try
            {
                // Validar si existe
                var rgro = ctx.TblTipoDocs.FirstOrDefault(x => x.Codigo == tblTipDoc.Codigo);
                if (rgro == null)
                {
                    return -1; // Error, no existe
                }
                // Actualiza cada dato
                rgro.Nombre = tblTipDoc.Nombre;
                rgro.Activo = tblTipDoc.Activo;
                ctx.SaveChanges();
                return tblTipDoc.Codigo;
            }
            catch
            {
                return 0; // Error de ejecución
            }
        }
    }
}
