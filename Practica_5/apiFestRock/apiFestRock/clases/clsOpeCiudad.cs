using apiFestRock.Models;

namespace apiFestRock.clases
{
    public class clsOpeCiudad
    {
        private readonly BdEventosContext ctx;

        public clsOpeCiudad(BdEventosContext ctx)
        {
            this.ctx = ctx;
        }

        //Propiedad de entrada y salida con los datos de la ciudad
        public TblCiudad tblCiud { get; set; }

        //Combo anidado: ciudades de un departamento
        public List<TblCiudad> listaCiudadesxDpto(int idDpto)
        {
            return ctx.TblCiudads
                .Where(x => x.IdDpto == idDpto)
                .OrderBy(x => x.Nombre)
                .ToList();
        }

        //iqueryable: consultar una ciudad por su nombre
        public IQueryable ciudadXNombre(string nombre)
        {
            return from IC in ctx.Set<TblCiudad>()
                   where IC.Nombre == nombre
                   select new {
                       codigo = IC.Codigo,
                       nombre = IC.Nombre,
                       idDep  = IC.IdDpto
                   };
        }

        //iqueryable: listar todas las ciudades para llenar la tabla
        public IQueryable listarCiudadsTabla()
        {
            return from IC in ctx.Set<TblCiudad>()
                   join ID in ctx.Set<TblDpto>() on IC.IdDpto equals ID.Codigo
                   orderby ID.Nombre, IC.Nombre
                   select new {
                       editar = "<a class='btn btn-info btn-sm' href=''><i class='fa fa-search-plus'></i> ir </a>",
                       codigo = IC.Codigo,
                       nombre = IC.Nombre,
                       dpto   = ID.Nombre
                   }; // Los 4 <th></th> para la tabla
        }

        //Agregar: retorna 0 = error de ejecución, -1 = ya existe el registro,
        //o el código (clave primaria) del registro ingresado
        public int Agregar()
        {
            try
            {
                // Validar si existe
                var existe = ctx.TblCiudads.FirstOrDefault(x => x.Nombre == tblCiud.Nombre && x.IdDpto == tblCiud.IdDpto);
                if (existe != null)
                {
                    return -1; // Ya existe
                }
                // Hallar el código
                int newCod = ctx.TblCiudads.Any() ? ctx.TblCiudads.Max(x => x.Codigo) : 0;
                tblCiud.Codigo = (newCod <= 0) ? 1 : newCod + 1;
                ctx.TblCiudads.Add(tblCiud);
                ctx.SaveChanges();
                return tblCiud.Codigo;
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
                var rgro = ctx.TblCiudads.FirstOrDefault(x => x.Codigo == tblCiud.Codigo);
                if (rgro == null)
                {
                    return -1; // Error, no existe
                }
                // Actualiza cada dato
                rgro.Nombre = tblCiud.Nombre;
                rgro.IdDpto = tblCiud.IdDpto;
                ctx.SaveChanges();
                return tblCiud.Codigo;
            }
            catch
            {
                return 0; // Error de ejecución
            }
        }
    }
}
