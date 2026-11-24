using apiLavanderia.Models;

namespace apiLavanderia.clases
{
    public class clsOpeProfesion
    {
        private readonly LavanderiaContext ctx;

        public clsOpeProfesion(LavanderiaContext ctx)
        {
            this.ctx = ctx;
        }

        //Propiedad de entrada y salida con los datos de la profesion
        public TblProfesion tblProfesion { get; set; }

        //Combo: profesiones
        public List<TblProfesion> listaProfesiones()
        {
            return ctx.TblProfesiones
                .OrderBy(x => x.NombreProfesion)
                .ToList();
        }

        //iqueryable: consultar una profesion por su nombre
        public IQueryable profesionXNombre(string nombre)
        {
            return from IP in ctx.Set<TblProfesion>()
                   where IP.NombreProfesion == nombre
                   select new {
                       codigo = IP.IdProfesion,
                       nombre = IP.NombreProfesion
                   };
        }

        //iqueryable: listar todas las profesiones para llenar la tabla
        public IQueryable listarProfesionesTabla()
        {
            return from IP in ctx.Set<TblProfesion>()
                   orderby IP.NombreProfesion
                   select new {
                       editar = "<a class='btn btn-info btn-sm' href=''><i class='fa fa-search-plus'></i> ir </a>",
                       codigo = IP.IdProfesion,
                       nombre = IP.NombreProfesion
                   }; // Los 3 <th></th> para la tabla
        }

        //Agregar: retorna 0 = error de ejecucion, -1 = ya existe el registro,
        //o el codigo (clave primaria) del registro ingresado
        public int Agregar()
        {
            try
            {
                // Validar si existe
                var existe = ctx.TblProfesiones.FirstOrDefault(x => x.NombreProfesion == tblProfesion.NombreProfesion);
                if (existe != null)
                {
                    return -1; // Ya existe
                }
                // Hallar el codigo
                int newCod = ctx.TblProfesiones.Any() ? ctx.TblProfesiones.Max(x => x.IdProfesion) : 0;
                tblProfesion.IdProfesion = (newCod <= 0) ? 1 : newCod + 1;
                ctx.TblProfesiones.Add(tblProfesion);
                ctx.SaveChanges();
                return tblProfesion.IdProfesion;
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
                var rgro = ctx.TblProfesiones.FirstOrDefault(x => x.IdProfesion == tblProfesion.IdProfesion);
                if (rgro == null)
                {
                    return -1; // Error, no existe
                }
                // Actualiza cada dato
                rgro.NombreProfesion = tblProfesion.NombreProfesion;
                ctx.SaveChanges();
                return tblProfesion.IdProfesion;
            }
            catch
            {
                return 0; // Error de ejecucion
            }
        }
    }
}
