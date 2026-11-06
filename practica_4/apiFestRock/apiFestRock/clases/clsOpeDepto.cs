using apiFestRock.Models;

namespace apiFestRock.clases
{
    public class clsOpeDepto
    {
        private readonly BdEventosContext ctx;

        public clsOpeDepto(BdEventosContext ctx)
        {
            this.ctx = ctx;
        }

        //Propiedad de entrada y salida con los datos del departamento
        public TblDpto tblDpto { get; set; }

        //Combo: departamentos
        public List<TblDpto> listaDptos()
        {
            return ctx.TblDptos
                .OrderBy(x => x.Nombre)
                .ToList();
        }

        //iqueryable: consultar un departamento por su nombre
        public IQueryable dptoXNombre(string nombre)
        {
            return from ID in ctx.Set<TblDpto>()
                   where ID.Nombre == nombre
                   select new {
                       codigo = ID.Codigo,
                       nombre = ID.Nombre
                   };
        }

        //iqueryable: listar todos los departamentos para llenar la tabla
        public IQueryable listarDptosTabla()
        {
            return from ID in ctx.Set<TblDpto>()
                   orderby ID.Nombre
                   select new {
                       editar = "<a class='btn btn-info btn-sm' href=''><i class='fa fa-search-plus'></i> ir </a>",
                       codigo = ID.Codigo,
                       nombre = ID.Nombre
                   }; // Los 3 <th></th> para la tabla
        }

        //Agregar: retorna 0 = error de ejecución, -1 = ya existe el registro,
        //o el código (clave primaria) del registro ingresado
        public int Agregar()
        {
            try
            {
                // Validar si existe
                var existe = ctx.TblDptos.FirstOrDefault(x => x.Nombre == tblDpto.Nombre);
                if (existe != null)
                {
                    return -1; // Ya existe
                }
                // Hallar el código
                int newCod = ctx.TblDptos.Any() ? ctx.TblDptos.Max(x => x.Codigo) : 0;
                tblDpto.Codigo = (newCod <= 0) ? 1 : newCod + 1;
                ctx.TblDptos.Add(tblDpto);
                ctx.SaveChanges();
                return tblDpto.Codigo;
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
                var rgro = ctx.TblDptos.FirstOrDefault(x => x.Codigo == tblDpto.Codigo);
                if (rgro == null)
                {
                    return -1; // Error, no existe
                }
                // Actualiza cada dato
                rgro.Nombre = tblDpto.Nombre;
                ctx.SaveChanges();
                return tblDpto.Codigo;
            }
            catch
            {
                return 0; // Error de ejecución
            }
        }
    }
}
