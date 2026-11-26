using apiLavanderia.Models;

namespace apiLavanderia.clases
{
    public class clsOpeUsuario
    {
        private readonly LavanderiaContext ctx;

        public clsOpeUsuario(LavanderiaContext ctx)
        {
            this.ctx = ctx;
        }

        //Propiedad de entrada y salida con los datos del usuario
        public TblUsuario tblUsuario { get; set; }

        //iqueryable: listar todos los usuarios (activos e inactivos) para llenar la tabla
        public IQueryable listarUsuariosTodos()
        {
            return from IU in ctx.Set<TblUsuario>()
                   join IE in ctx.Set<TblEmpleado>() on IU.EmpleadoIdEmpleado equals IE.IdEmpleado
                   orderby IU.Usuario
                   select new {
                       codigo   = IU.IdUsuarios,
                       idEmp    = IU.EmpleadoIdEmpleado,
                       empleado = IE.NombreApellidoEmp,
                       usuario  = IU.Usuario,
                       clave    = IU.Clave,
                       act      = IU.Activo
                   };
        }

        //iqueryable: validar el login de un usuario (usuario + clave)
        public IQueryable validarLogin(string usuario, string clave)
        {
            return from IU in ctx.Set<TblUsuario>()
                   join IE in ctx.Set<TblEmpleado>() on IU.EmpleadoIdEmpleado equals IE.IdEmpleado
                   where IU.Usuario == usuario && IU.Clave == clave && IU.Activo == true
                   select new {
                       idUsuario = IU.IdUsuarios,
                       idEmpleado = IU.EmpleadoIdEmpleado,
                       empleado  = IE.NombreApellidoEmp,
                       usuario   = IU.Usuario,
                       activo    = IU.Activo
                   };
        }

        //iqueryable: consultar un usuario por su nombre de usuario
        public IQueryable usuarioXNombre(string nombre)
        {
            return from IU in ctx.Set<TblUsuario>()
                   join IE in ctx.Set<TblEmpleado>() on IU.EmpleadoIdEmpleado equals IE.IdEmpleado
                   where IU.Usuario == nombre
                   select new {
                       codigo   = IU.IdUsuarios,
                       idEmp    = IU.EmpleadoIdEmpleado,
                       empleado = IE.NombreApellidoEmp,
                       clave = IU.Clave,
                       usuario  = IU.Usuario,
                       act      = IU.Activo
                   };
        }

        //Agregar: retorna 0 = error de ejecucion, -1 = ya existe el registro,
        //o el codigo (clave primaria) del registro ingresado
        public int Agregar()
        {
            try
            {
                // Validar si existe
                var existe = ctx.TblUsuarios.FirstOrDefault(x => x.Usuario == tblUsuario.Usuario);
                if (existe != null)
                {
                    return -1; // Ya existe
                }
                // Hallar el codigo
                int newCod = ctx.TblUsuarios.Any() ? ctx.TblUsuarios.Max(x => x.IdUsuarios) : 0;
                tblUsuario.IdUsuarios = (newCod <= 0) ? 1 : newCod + 1;
                ctx.TblUsuarios.Add(tblUsuario);
                ctx.SaveChanges();
                return tblUsuario.IdUsuarios;
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
                var rgro = ctx.TblUsuarios.FirstOrDefault(x => x.IdUsuarios == tblUsuario.IdUsuarios);
                if (rgro == null)
                {
                    return -1; // Error, no existe
                }
                // Actualiza cada dato
                rgro.EmpleadoIdEmpleado = tblUsuario.EmpleadoIdEmpleado;
                rgro.Usuario = tblUsuario.Usuario;
                rgro.Clave = tblUsuario.Clave;
                rgro.Activo = tblUsuario.Activo;
                ctx.SaveChanges();
                return tblUsuario.IdUsuarios;
            }
            catch
            {
                return 0; // Error de ejecucion
            }
        }
    }
}
