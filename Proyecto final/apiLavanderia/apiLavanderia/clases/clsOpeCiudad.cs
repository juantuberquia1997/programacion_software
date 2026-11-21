using apiLavanderia.Models;

namespace apiLavanderia.clases
{
    public class clsOpeCiudad
    {
        private readonly LavanderiaContext ctx;

        public clsOpeCiudad(LavanderiaContext ctx)
        {
            this.ctx = ctx;
        }

        //Propiedad de entrada y salida con los datos de la ciudad
        public TblCiudad tblCiudad { get; set; }

        //Combo anidado: ciudades de un departamento
        public List<TblCiudad> listaCiudadesxDpto(int idDpto)
        {
            return ctx.TblCiudades
                .Where(x => x.IdDepartamento == idDpto)
                .OrderBy(x => x.NombreCiudad)
                .ToList();
        }

        //iqueryable: consultar una ciudad por su nombre
        public IQueryable ciudadXNombre(string nombre)
        {
            return from IC in ctx.Set<TblCiudad>()
                   where IC.NombreCiudad == nombre
                   select new {
                       codigo = IC.IdCiudad,
                       nombre = IC.NombreCiudad,
                       idDep  = IC.IdDepartamento
                   };
        }

        //iqueryable: listar todas las ciudades para llenar la tabla
        public IQueryable listarCiudadesTabla()
        {
            return from IC in ctx.Set<TblCiudad>()
                   join ID in ctx.Set<TblDepartamento>() on IC.IdDepartamento equals ID.IdDepartamento
                   orderby ID.NombreDepartamento, IC.NombreCiudad
                   select new {
                       editar = "<a class='btn btn-info btn-sm' href=''><i class='fa fa-search-plus'></i> ir </a>",
                       codigo = IC.IdCiudad,
                       nombre = IC.NombreCiudad,
                       dpto   = ID.NombreDepartamento
                   }; // Los 4 <th></th> para la tabla
        }

        //Agregar: retorna 0 = error de ejecucion, -1 = ya existe el registro,
        //o el codigo (clave primaria) del registro ingresado
        public int Agregar()
        {
            try
            {
                // Validar si existe
                var existe = ctx.TblCiudades.FirstOrDefault(x => x.NombreCiudad == tblCiudad.NombreCiudad && x.IdDepartamento == tblCiudad.IdDepartamento);
                if (existe != null)
                {
                    return -1; // Ya existe
                }
                // Hallar el codigo
                int newCod = ctx.TblCiudades.Any() ? ctx.TblCiudades.Max(x => x.IdCiudad) : 0;
                tblCiudad.IdCiudad = (newCod <= 0) ? 1 : newCod + 1;
                ctx.TblCiudades.Add(tblCiudad);
                ctx.SaveChanges();
                return tblCiudad.IdCiudad;
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
                var rgro = ctx.TblCiudades.FirstOrDefault(x => x.IdCiudad == tblCiudad.IdCiudad);
                if (rgro == null)
                {
                    return -1; // Error, no existe
                }
                // Actualiza cada dato
                rgro.NombreCiudad = tblCiudad.NombreCiudad;
                rgro.IdDepartamento = tblCiudad.IdDepartamento;
                ctx.SaveChanges();
                return tblCiudad.IdCiudad;
            }
            catch
            {
                return 0; // Error de ejecucion
            }
        }
    }
}
