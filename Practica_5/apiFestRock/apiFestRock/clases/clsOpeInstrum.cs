using apiFestRock.Models;

namespace apiFestRock.clases
{
    public class clsOpeInstrum
    {
        private readonly BdEventosContext ctx;

        public clsOpeInstrum(BdEventosContext ctx)
        {
            this.ctx = ctx;
        }

        //Propiedad de entrada y salida con los datos del instrumento
        public TblInstrum tblInstr { get; set; }

        //Combo: instrumentos activos (cboInstrum de frmBanda y frmInscrip)
        public List<TblInstrum> listaInstrums()
        {
            return ctx.TblInstrums
                .Where(x => x.Activo == true)
                .OrderBy(x => x.Nombre)
                .ToList();
        }

        //iqueryable: consultar un instrumento por su nombre
        public IQueryable instrumXNombre(string nombre)
        {
            return from II in ctx.Set<TblInstrum>()
                   where II.Nombre == nombre
                   select new {
                       codigo = II.Codigo,
                       nombre = II.Nombre,
                       act    = II.Activo
                   };
        }

        //iqueryable: listar todos los instrumentos para llenar la tabla
        public IQueryable listarInstrumsTabla()
        {
            return from II in ctx.Set<TblInstrum>()
                   orderby II.Nombre
                   select new {
                       editar = "<a class='btn btn-info btn-sm' href=''><i class='fa fa-search-plus'></i> ir </a>",
                       codigo = II.Codigo,
                       nombre = II.Nombre,
                       activo = II.Activo
                   }; // Los 4 <th></th> para la tabla
        }

        //Agregar: retorna 0 = error de ejecución, -1 = ya existe el registro,
        //o el código (clave primaria) del registro ingresado
        public int Agregar()
        {
            try
            {
                // Validar si existe
                var existe = ctx.TblInstrums.FirstOrDefault(x => x.Nombre == tblInstr.Nombre);
                if (existe != null)
                {
                    return -1; // Ya existe
                }
                // Hallar el código
                int newCod = ctx.TblInstrums.Any() ? ctx.TblInstrums.Max(x => x.Codigo) : 0;
                tblInstr.Codigo = (newCod <= 0) ? 1 : newCod + 1;
                ctx.TblInstrums.Add(tblInstr);
                ctx.SaveChanges();
                return tblInstr.Codigo;
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
                var rgro = ctx.TblInstrums.FirstOrDefault(x => x.Codigo == tblInstr.Codigo);
                if (rgro == null)
                {
                    return -1; // Error, no existe
                }
                // Actualiza cada dato
                rgro.Nombre = tblInstr.Nombre;
                rgro.Activo = tblInstr.Activo;
                ctx.SaveChanges();
                return tblInstr.Codigo;
            }
            catch
            {
                return 0; // Error de ejecución
            }
        }
    }
}
