using apiLavanderia.Models;

namespace apiLavanderia.clases
{
    //Clase de entrada para guardar el cliente con sus direcciones y telefonos
    public class clsClienteIn
    {
        public TblCliente tblCliente { get; set; } = new TblCliente();
        public List<TblDireccionCliente> direcciones { get; set; } = new List<TblDireccionCliente>();
        public List<TblTelefonoCliente> telefonos { get; set; } = new List<TblTelefonoCliente>();
    }

    public class clsOpeCliente
    {
        private readonly LavanderiaContext ctx;

        public clsOpeCliente(LavanderiaContext ctx)
        {
            this.ctx = ctx;
        }

        //Combo: clientes (cboCliente de los formularios)
        public List<TblCliente> listaClientes()
        {
            return ctx.TblClientes
                .OrderBy(x => x.NombreApellidoCliente)
                .ToList();
        }

        //iqueryable: consultar un cliente por su numero de documento
        public IQueryable clienteXDocumento(string documento)
        {
            return from IC in ctx.Set<TblCliente>()
                   join ITD in ctx.Set<TblTipoDocumento>() on IC.IdTipoDocumento equals ITD.IdTipoDocumento
                   join IE in ctx.Set<TblEmpleado>() on IC.EmpleadoIdEmpleado equals IE.IdEmpleado
                   where IC.NumeroDocumento == documento
                   select new {
                       codigo   = IC.IdCliente,
                       empleado = IE.IdEmpleado,
                       nombre   = IC.NombreApellidoCliente,
                       idTD     = IC.IdTipoDocumento,
                       tipoDoc  = ITD.NombreDocumento,
                       documento = IC.NumeroDocumento
                   };
        }

        //iqueryable: listar todos los clientes para llenar la tabla
        public IQueryable listarClientesTabla()
        {
            return from IC in ctx.Set<TblCliente>()
                   join ITD in ctx.Set<TblTipoDocumento>() on IC.IdTipoDocumento equals ITD.IdTipoDocumento
                   orderby IC.NombreApellidoCliente
                   select new {
                       editar   = "<a class='btn btn-info btn-sm' href=''><i class='fa fa-search-plus'></i> ir </a>",
                       codigo   = IC.IdCliente,
                       nombre   = IC.NombreApellidoCliente,
                       tipoDoc  = ITD.NombreDocumento,
                       documento = IC.NumeroDocumento
                   }; // Los 5 <th></th> para la tabla
        }

        //iqueryable: direcciones de un cliente (detalle)
        public IQueryable direccionesXCliente(int idCliente)
        {
            return from DC in ctx.Set<TblDireccionCliente>()
                   join IC in ctx.Set<TblCiudad>() on DC.IdCiudad equals IC.IdCiudad
                   where DC.IdCliente == idCliente
                   orderby DC.IdDireccion
                   select new {
                       idDir     = DC.IdDireccion,
                       idCiudad  = DC.IdCiudad,
                       ciudad    = IC.NombreCiudad,
                       direccion = DC.DireccionCliente
                   };
        }

        //iqueryable: telefonos de un cliente (detalle)
        public IQueryable telefonosXCliente(int idCliente)
        {
            return from TC in ctx.Set<TblTelefonoCliente>()
                   join IT in ctx.Set<TblTipoTelefono>() on TC.IdTipoTelefono equals IT.IdTipoTelefono
                   where TC.IdCliente == idCliente
                   orderby TC.IdTelefono
                   select new {
                       idTel      = TC.IdTelefono,
                       idTipoTel  = TC.IdTipoTelefono,
                       tipoTel    = IT.TipoTelefono,
                       numero     = TC.Numero
                   };
        }

        //Agregar cliente con su detalle (direcciones y telefonos) en una sola
        //transaccion: retorna 0 = error de ejecucion, -1 = ya existe el registro,
        //o el codigo (clave primaria) del registro ingresado
        public int Agregar(clsClienteIn datIn)
        {
            try
            {
                TblCliente tblCliente = datIn.tblCliente;
                // Validar si existe
                var existe = ctx.TblClientes.FirstOrDefault(x => x.NumeroDocumento == tblCliente.NumeroDocumento);
                if (existe != null)
                {
                    return -1; // Ya existe
                }
                // Hallar el codigo
                int newCod = ctx.TblClientes.Any() ? ctx.TblClientes.Max(x => x.IdCliente) : 0;
                tblCliente.IdCliente = (newCod <= 0) ? 1 : newCod + 1;
                // Transaccion: primero el encabezado, luego el detalle
                using (var transaccion = ctx.Database.BeginTransaction())
                {
                    ctx.TblClientes.Add(tblCliente);
                    ctx.SaveChanges();
                    int maxDir = ctx.TblDireccionClientes.Any() ? ctx.TblDireccionClientes.Max(x => x.IdDireccion) : 0;
                    foreach (var dir in datIn.direcciones)
                    {
                        maxDir = maxDir + 1;
                        dir.IdDireccion = maxDir;
                        dir.IdCliente = tblCliente.IdCliente;
                        ctx.TblDireccionClientes.Add(dir);
                    }
                    int maxTel = ctx.TblTelefonoClientes.Any() ? ctx.TblTelefonoClientes.Max(x => x.IdTelefono) : 0;
                    foreach (var tel in datIn.telefonos)
                    {
                        maxTel = maxTel + 1;
                        tel.IdTelefono = maxTel;
                        tel.IdCliente = tblCliente.IdCliente;
                        ctx.TblTelefonoClientes.Add(tel);
                    }
                    ctx.SaveChanges();
                    transaccion.Commit();
                }
                return tblCliente.IdCliente;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error Agregar cliente: " + ex.Message);
                return 0; // Error de ejecucion
            }
        }

        //Modificar cliente y actualizar su detalle: retorna 0 = error de
        //ejecucion, -1 = no existe el registro, o el codigo (clave primaria)
        //del registro modificado
        public int Modificar(clsClienteIn datIn)
        {
            try
            {
                TblCliente tblCliente = datIn.tblCliente;
                // Validar si existe
                var rgro = ctx.TblClientes.FirstOrDefault(x => x.IdCliente == tblCliente.IdCliente);
                if (rgro == null)
                {
                    return -1; // Error, no existe
                }
                // Actualiza cada dato
                rgro.EmpleadoIdEmpleado = tblCliente.EmpleadoIdEmpleado;
                rgro.IdTipoDocumento = tblCliente.IdTipoDocumento;
                rgro.NombreApellidoCliente = tblCliente.NombreApellidoCliente;
                rgro.NumeroDocumento = tblCliente.NumeroDocumento;
                // Actualizar direcciones: no se eliminan, se actualizan o adicionan
                int maxDir = ctx.TblDireccionClientes.Any() ? ctx.TblDireccionClientes.Max(x => x.IdDireccion) : 0;
                foreach (var dir in datIn.direcciones)
                {
                    if (dir.IdDireccion > 0)
                    {
                        var rgroDir = ctx.TblDireccionClientes.FirstOrDefault(x => x.IdDireccion == dir.IdDireccion && x.IdCliente == rgro.IdCliente);
                        if (rgroDir != null)
                        {
                            rgroDir.IdCiudad = dir.IdCiudad;
                            rgroDir.DireccionCliente = dir.DireccionCliente;
                        }
                    }
                    else
                    {
                        maxDir = maxDir + 1;
                        dir.IdDireccion = maxDir;
                        dir.IdCliente = rgro.IdCliente;
                        ctx.TblDireccionClientes.Add(dir);
                    }
                }
                // Actualizar telefonos: no se eliminan, se actualizan o adicionan
                int maxTel = ctx.TblTelefonoClientes.Any() ? ctx.TblTelefonoClientes.Max(x => x.IdTelefono) : 0;
                foreach (var tel in datIn.telefonos)
                {
                    if (tel.IdTelefono > 0)
                    {
                        var rgroTel = ctx.TblTelefonoClientes.FirstOrDefault(x => x.IdTelefono == tel.IdTelefono && x.IdCliente == rgro.IdCliente);
                        if (rgroTel != null)
                        {
                            rgroTel.IdTipoTelefono = tel.IdTipoTelefono;
                            rgroTel.Numero = tel.Numero;
                        }
                    }
                    else
                    {
                        maxTel = maxTel + 1;
                        tel.IdTelefono = maxTel;
                        tel.IdCliente = rgro.IdCliente;
                        ctx.TblTelefonoClientes.Add(tel);
                    }
                }
                ctx.SaveChanges();
                return rgro.IdCliente;
            }
            catch
            {
                return 0; // Error de ejecucion
            }
        }
    }
}
