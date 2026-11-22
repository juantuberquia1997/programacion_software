using apiLavanderia.Models;

namespace apiLavanderia.clases
{
    //Clase de entrada para guardar el empleado con sus profesiones,
    //direcciones y telefonos
    public class clsEmpleadoIn
    {
        public TblEmpleado tblEmpleado { get; set; } = new TblEmpleado();
        public List<int> profesiones { get; set; } = new List<int>();
        public List<TblDireccionEmpleado> direcciones { get; set; } = new List<TblDireccionEmpleado>();
        public List<TblTelefonoEmpleado> telefonos { get; set; } = new List<TblTelefonoEmpleado>();
    }

    public class clsOpeEmpleado
    {
        private readonly LavanderiaContext ctx;

        public clsOpeEmpleado(LavanderiaContext ctx)
        {
            this.ctx = ctx;
        }

        //Combo: empleados activos (cboEmpleado de los formularios)
        public List<TblEmpleado> listaEmpleados()
        {
            return ctx.TblEmpleados
                .Where(x => x.Activo == true)
                .OrderBy(x => x.NombreApellidoEmp)
                .ToList();
        }

        //iqueryable: consultar un empleado por su numero de documento
        public IQueryable empleadoXDocumento(string documento)
        {
            return from IE in ctx.Set<TblEmpleado>()
                   join ITD in ctx.Set<TblTipoDocumento>() on IE.IdTipoDocumento equals ITD.IdTipoDocumento
                   join IEE in ctx.Set<TblEmpleado>() on IE.EmpleadoIdEmpleado equals IEE.IdEmpleado
                   where IE.DocumentoEmpleado == documento
                   select new {
                       codigo     = IE.IdEmpleado,
                       empleado = IEE.IdEmpleado,
                       nombre     = IE.NombreApellidoEmp,
                       idTD       = IE.IdTipoDocumento,
                       //tipoDoc    = ITD.NombreDocumento,
                       documento  = IE.DocumentoEmpleado,
                       fechaIng   = IE.FechaIngreso,
                       salario    = IE.Salario,
                       act        = IE.Activo
                   };
        }

        //iqueryable: listar todos los empleados para llenar la tabla
        public IQueryable listarEmpleadosTabla()
        {
            return from IE in ctx.Set<TblEmpleado>()
                   join ITD in ctx.Set<TblTipoDocumento>() on IE.IdTipoDocumento equals ITD.IdTipoDocumento
                   orderby IE.NombreApellidoEmp
                   select new {
                       editar   = "<a class='btn btn-info btn-sm' href=''><i class='fa fa-search-plus'></i> ir </a>",
                       codigo   = IE.IdEmpleado,
                       nombre   = IE.NombreApellidoEmp,
                       tipoDoc  = ITD.NombreDocumento,
                       documento = IE.DocumentoEmpleado,
                       salario  = IE.Salario,
                       activo   = IE.Activo
                   }; // Los 7 <th></th> para la tabla
        }

        //iqueryable: profesiones de un empleado (detalle)
        public IQueryable profesionesXEmpleado(int idEmpleado)
        {
            return from HP in ctx.Set<TblEmpleadoHasProfesion>()
                   join IP in ctx.Set<TblProfesion>() on HP.ProfesionIdProfesion equals IP.IdProfesion
                   where HP.EmpleadoIdEmpleado == idEmpleado
                   orderby IP.NombreProfesion
                   select new {
                       idProfesion = IP.IdProfesion,
                       profesion   = IP.NombreProfesion
                   };
        }

        //iqueryable: direcciones de un empleado (detalle)
        public IQueryable direccionesXEmpleado(int idEmpleado)
        {
            return from DE in ctx.Set<TblDireccionEmpleado>()
                   join IC in ctx.Set<TblCiudad>() on DE.IdCiudad equals IC.IdCiudad
                   where DE.EmpleadoIdEmpleado == idEmpleado
                   orderby DE.IdDireccionEmpleado
                   select new {
                       idDir     = DE.IdDireccionEmpleado,
                       idCiudad  = DE.IdCiudad,
                       ciudad    = IC.NombreCiudad,
                       direccion = DE.DireccionEmpleado
                   };
        }

        //iqueryable: telefonos de un empleado (detalle)
        public IQueryable telefonosXEmpleado(int idEmpleado)
        {
            return from TE in ctx.Set<TblTelefonoEmpleado>()
                   join IT in ctx.Set<TblTipoTelefono>() on TE.IdTipoTelefono equals IT.IdTipoTelefono
                   where TE.EmpleadoIdEmpleado == idEmpleado
                   orderby TE.IdTelefonoEmpleado
                   select new {
                       idTel      = TE.IdTelefonoEmpleado,
                       idTipoTel  = TE.IdTipoTelefono,
                       tipoTel    = IT.TipoTelefono,
                       numero     = TE.Numero
                   };
        }

        //Agregar una direccion a un empleado existente: retorna 0 = error de
        //ejecucion, -1 = no existe el empleado, o el codigo de la direccion
        public int AgregarDireccion(TblDireccionEmpleado dir)
        {
            try
            {
                var rgro = ctx.TblEmpleados.FirstOrDefault(x => x.IdEmpleado == dir.EmpleadoIdEmpleado);
                if (rgro == null)
                {
                    return -1; // Error, no existe el empleado
                }
                int maxDir = ctx.TblDireccionEmpleados.Any() ? ctx.TblDireccionEmpleados.Max(x => x.IdDireccionEmpleado) : 0;
                dir.IdDireccionEmpleado = maxDir + 1;
                ctx.TblDireccionEmpleados.Add(dir);
                ctx.SaveChanges();
                return dir.IdDireccionEmpleado;
            }
            catch
            {
                return 0; // Error de ejecucion
            }
        }

        //Agregar un telefono a un empleado existente: retorna 0 = error de
        //ejecucion, -1 = no existe el empleado, o el codigo del telefono
        public int AgregarTelefono(TblTelefonoEmpleado tel)
        {
            try
            {
                var rgro = ctx.TblEmpleados.FirstOrDefault(x => x.IdEmpleado == tel.EmpleadoIdEmpleado);
                if (rgro == null)
                {
                    return -1; // Error, no existe el empleado
                }
                int maxTel = ctx.TblTelefonoEmpleados.Any() ? ctx.TblTelefonoEmpleados.Max(x => x.IdTelefonoEmpleado) : 0;
                tel.IdTelefonoEmpleado = maxTel + 1;
                ctx.TblTelefonoEmpleados.Add(tel);
                ctx.SaveChanges();
                return tel.IdTelefonoEmpleado;
            }
            catch
            {
                return 0; // Error de ejecucion
            }
        }

        //Modificar una direccion de un empleado: retorna 0 = error de ejecucion,
        //-1 = no existe la direccion, o el codigo de la direccion modificada
        public int ModificarDireccion(TblDireccionEmpleado dir)
        {
            try
            {
                var rgro = ctx.TblDireccionEmpleados.FirstOrDefault(x => x.IdDireccionEmpleado == dir.IdDireccionEmpleado && x.EmpleadoIdEmpleado == dir.EmpleadoIdEmpleado);
                if (rgro == null)
                {
                    return -1; // Error, no existe la direccion
                }
                rgro.IdCiudad = dir.IdCiudad;
                rgro.DireccionEmpleado = dir.DireccionEmpleado;
                ctx.SaveChanges();
                return rgro.IdDireccionEmpleado;
            }
            catch
            {
                return 0; // Error de ejecucion
            }
        }

        //Modificar un telefono de un empleado: retorna 0 = error de ejecucion,
        //-1 = no existe el telefono, o el codigo del telefono modificado
        public int ModificarTelefono(TblTelefonoEmpleado tel)
        {
            try
            {
                var rgro = ctx.TblTelefonoEmpleados.FirstOrDefault(x => x.IdTelefonoEmpleado == tel.IdTelefonoEmpleado && x.EmpleadoIdEmpleado == tel.EmpleadoIdEmpleado);
                if (rgro == null)
                {
                    return -1; // Error, no existe el telefono
                }
                rgro.IdTipoTelefono = tel.IdTipoTelefono;
                rgro.Numero = tel.Numero;
                ctx.SaveChanges();
                return rgro.IdTelefonoEmpleado;
            }
            catch
            {
                return 0; // Error de ejecucion
            }
        }

        //Agregar empleado con su detalle (profesiones, direcciones y telefonos)
        //en una sola transaccion: retorna 0 = error de ejecucion, -1 = ya existe
        //el registro, o el codigo (clave primaria) del registro ingresado
        public int Agregar(clsEmpleadoIn datIn)
        {
            try
            {
                TblEmpleado tblEmpleado = datIn.tblEmpleado;
                // Validar si existe
                var existe = ctx.TblEmpleados.FirstOrDefault(x => x.DocumentoEmpleado == tblEmpleado.DocumentoEmpleado);
                if (existe != null)
                {
                    return -1; // Ya existe
                }
                // Hallar el codigo
                int newCod = ctx.TblEmpleados.Any() ? ctx.TblEmpleados.Max(x => x.IdEmpleado) : 0;
                tblEmpleado.IdEmpleado = (newCod <= 0) ? 1 : newCod + 1;
                // Transaccion: primero el encabezado, luego el detalle
                //(el modelo no tiene las relaciones, EF no garantiza el orden entre tablas)
                using (var transaccion = ctx.Database.BeginTransaction())
                {
                    ctx.TblEmpleados.Add(tblEmpleado);
                    ctx.SaveChanges();
                    foreach (var idProf in datIn.profesiones)
                    {
                        ctx.TblEmpleadoHasProfesiones.Add(new TblEmpleadoHasProfesion
                        {
                            EmpleadoIdEmpleado = tblEmpleado.IdEmpleado,
                            ProfesionIdProfesion = idProf
                        });
                    }
                    int maxDir = ctx.TblDireccionEmpleados.Any() ? ctx.TblDireccionEmpleados.Max(x => x.IdDireccionEmpleado) : 0;
                    foreach (var dir in datIn.direcciones)
                    {
                        maxDir = maxDir + 1;
                        dir.IdDireccionEmpleado = maxDir;
                        dir.EmpleadoIdEmpleado = tblEmpleado.IdEmpleado;
                        ctx.TblDireccionEmpleados.Add(dir);
                    }
                    int maxTel = ctx.TblTelefonoEmpleados.Any() ? ctx.TblTelefonoEmpleados.Max(x => x.IdTelefonoEmpleado) : 0;
                    foreach (var tel in datIn.telefonos)
                    {
                        maxTel = maxTel + 1;
                        tel.IdTelefonoEmpleado = maxTel;
                        tel.EmpleadoIdEmpleado = tblEmpleado.IdEmpleado;
                        ctx.TblTelefonoEmpleados.Add(tel);
                    }
                    ctx.SaveChanges();
                    transaccion.Commit();
                }
                return tblEmpleado.IdEmpleado;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error Agregar empleado: " + ex.Message);
                return 0; // Error de ejecucion
            }
        }

        //Modificar empleado y actualizar su detalle: retorna 0 = error de
        //ejecucion, -1 = no existe el registro, o el codigo (clave primaria)
        //del registro modificado
        public int Modificar(clsEmpleadoIn datIn)
        {
            try
            {
                TblEmpleado tblEmpleado = datIn.tblEmpleado;
                // Validar si existe
                var rgro = ctx.TblEmpleados.FirstOrDefault(x => x.IdEmpleado == tblEmpleado.IdEmpleado);
                if (rgro == null)
                {
                    return -1; // Error, no existe
                }
                // Actualiza cada dato
                rgro.EmpleadoIdEmpleado = tblEmpleado.EmpleadoIdEmpleado;
                rgro.IdTipoDocumento = tblEmpleado.IdTipoDocumento;
                rgro.NombreApellidoEmp = tblEmpleado.NombreApellidoEmp;
                rgro.DocumentoEmpleado = tblEmpleado.DocumentoEmpleado;
                rgro.Activo = tblEmpleado.Activo;
                rgro.FechaIngreso = tblEmpleado.FechaIngreso;
                rgro.Salario = tblEmpleado.Salario;
                // Reemplazar las profesiones del empleado
                var profAnt = ctx.TblEmpleadoHasProfesiones.Where(x => x.EmpleadoIdEmpleado == rgro.IdEmpleado).ToList();
                ctx.TblEmpleadoHasProfesiones.RemoveRange(profAnt);
                foreach (var idProf in datIn.profesiones)
                {
                    ctx.TblEmpleadoHasProfesiones.Add(new TblEmpleadoHasProfesion
                    {
                        EmpleadoIdEmpleado = rgro.IdEmpleado,
                        ProfesionIdProfesion = idProf
                    });
                }
                // Actualizar direcciones: no se eliminan, se actualizan o adicionan
                int maxDir = ctx.TblDireccionEmpleados.Any() ? ctx.TblDireccionEmpleados.Max(x => x.IdDireccionEmpleado) : 0;
                foreach (var dir in datIn.direcciones)
                {
                    if (dir.IdDireccionEmpleado > 0)
                    {
                        var rgroDir = ctx.TblDireccionEmpleados.FirstOrDefault(x => x.IdDireccionEmpleado == dir.IdDireccionEmpleado && x.EmpleadoIdEmpleado == rgro.IdEmpleado);
                        if (rgroDir != null)
                        {
                            rgroDir.IdCiudad = dir.IdCiudad;
                            rgroDir.DireccionEmpleado = dir.DireccionEmpleado;
                        }
                    }
                    else
                    {
                        maxDir = maxDir + 1;
                        dir.IdDireccionEmpleado = maxDir;
                        dir.EmpleadoIdEmpleado = rgro.IdEmpleado;
                        ctx.TblDireccionEmpleados.Add(dir);
                    }
                }
                // Actualizar telefonos: no se eliminan, se actualizan o adicionan
                int maxTel = ctx.TblTelefonoEmpleados.Any() ? ctx.TblTelefonoEmpleados.Max(x => x.IdTelefonoEmpleado) : 0;
                foreach (var tel in datIn.telefonos)
                {
                    if (tel.IdTelefonoEmpleado > 0)
                    {
                        var rgroTel = ctx.TblTelefonoEmpleados.FirstOrDefault(x => x.IdTelefonoEmpleado == tel.IdTelefonoEmpleado && x.EmpleadoIdEmpleado == rgro.IdEmpleado);
                        if (rgroTel != null)
                        {
                            rgroTel.IdTipoTelefono = tel.IdTipoTelefono;
                            rgroTel.Numero = tel.Numero;
                        }
                    }
                    else
                    {
                        maxTel = maxTel + 1;
                        tel.IdTelefonoEmpleado = maxTel;
                        tel.EmpleadoIdEmpleado = rgro.IdEmpleado;
                        ctx.TblTelefonoEmpleados.Add(tel);
                    }
                }
                ctx.SaveChanges();
                return rgro.IdEmpleado;
            }
            catch
            {
                return 0; // Error de ejecucion
            }
        }
    }
}
