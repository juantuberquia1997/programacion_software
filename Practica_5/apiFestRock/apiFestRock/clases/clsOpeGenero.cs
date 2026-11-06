using apiFestRock.Models;

namespace apiFestRock.clases
{
    public class clsOpeGenero
    {
        private readonly BdEventosContext ctx;

        public clsOpeGenero(BdEventosContext ctx)
        {
            this.ctx = ctx;
        }

        //Propiedad de entrada y salida con los datos del género
        public TblGenero tblGene { get; set; }

        //Combo: géneros activos
        public List<TblGenero> listaGeneros() {
            return ctx.TblGeneros
                .Where(x => x.Activo == true)
                .OrderBy(x => x.Nombre)
                .ToList();
        }

        //iqueryable: consultar un género por su nombre
        public IQueryable generoXNombre(string nombre)
        {
            return from IG in ctx.Set<TblGenero>()
                   where IG.Nombre == nombre
                   select new {
                       codigo = IG.Codigo,
                       nombre = IG.Nombre,
                       act    = IG.Activo
                   };
        }

        //iqueryable: listar todos los géneros para llenar la tabla
        public IQueryable listarGenerosTabla()
        {
            return from IG in ctx.Set<TblGenero>()
                   orderby IG.Nombre
                   select new {
                       editar = "<a class='btn btn-info btn-sm' href=''><i class='fa fa-search-plus'></i> ir </a>",
                       codigo = IG.Codigo,
                       nombre = IG.Nombre,
                       activo = IG.Activo
                   }; // Los 4 <th></th> para la tabla
        }

        //Agregar: retorna 0 = error de ejecución, -1 = ya existe el registro,
        //o el código (clave primaria) del registro ingresado
        public int Agregar()
        {
            try
            {
                // Validar si existe
                var existe = ctx.TblGeneros.FirstOrDefault(x => x.Nombre == tblGene.Nombre);
                if (existe != null)
                {
                    return -1; // Ya existe
                }
                // Hallar el código
                int newCod = ctx.TblGeneros.Any() ? ctx.TblGeneros.Max(x => x.Codigo) : 0;
                tblGene.Codigo = (newCod <= 0) ? 1 : newCod + 1;
                ctx.TblGeneros.Add(tblGene);
                ctx.SaveChanges();
                return tblGene.Codigo;
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
                var rgro = ctx.TblGeneros.FirstOrDefault(x => x.Codigo == tblGene.Codigo);
                if (rgro == null)
                {
                    return -1; // Error, no existe
                }
                // Actualiza cada dato
                rgro.Nombre = tblGene.Nombre;
                rgro.Activo = tblGene.Activo;
                ctx.SaveChanges();
                return tblGene.Codigo;
            }
            catch
            {
                return 0; // Error de ejecución
            }
        }
    }
}
