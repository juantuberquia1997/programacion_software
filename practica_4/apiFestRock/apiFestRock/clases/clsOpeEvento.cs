using apiFestRock.Models;

namespace apiFestRock.clases
{
    public class clsOpeEvento
    {
        private readonly BdEventosContext ctx;

        public clsOpeEvento(BdEventosContext ctx)
        {
            this.ctx = ctx;
        }

        //Propiedad de entrada y salida con los datos del evento
        public TblEvento tblEvent { get; set; }

        //Combo: eventos activos (cboEvento del form Inscripción)
        public List<TblEvento> listaEventos()
        {
            return ctx.TblEventos
                .Where(x => x.Activo == true)
                .OrderBy(x => x.Nombre)
                .ToList();
        }

        //iqueryable: valor de la inscripción por artista de un evento (lblVrIndInsc)
        public IQueryable consultarVrInscrip(int idEvent)
        {
            return from IE in ctx.Set<TblEvento>()
                   where IE.Codigo == idEvent
                   select new {
                       codigo = IE.Codigo,
                       nombre = IE.Nombre,
                       vrInsc = IE.VrInscEvent
                   };
        }

        //iqueryable: consultar un evento por su nombre
        public IQueryable eventoXNombre(string nombre)
        {
            return from IE in ctx.Set<TblEvento>()
                   join IC in ctx.Set<TblCiudad>() on IE.IdCiudad equals IC.Codigo
                   where IE.Nombre == nombre
                   select new {
                       codigo = IE.Codigo,
                       nombre = IE.Nombre,
                       idDep  = IC.IdDpto,
                       idC    = IE.IdCiudad,
                       anio   = IE.Anio,
                       fecha  = IE.Fecha,
                       lugar  = IE.Lugar,
                       vrInsc = IE.VrInscEvent,
                       act    = IE.Activo
                   };
        }

        //iqueryable: listar todos los eventos para llenar la tabla
        public IQueryable listarEventosTabla()
        {
            return from IE in ctx.Set<TblEvento>()
                   join IC in ctx.Set<TblCiudad>() on IE.IdCiudad equals IC.Codigo
                   join ID in ctx.Set<TblDpto>() on IC.IdDpto equals ID.Codigo
                   orderby IE.Nombre
                   select new {
                       editar   = "<a class='btn btn-info btn-sm' href=''><i class='fa fa-search-plus'></i> ir </a>",
                       codigo   = IE.Codigo,
                       nombre   = IE.Nombre,
                       dpto     = ID.Nombre,
                       ciudad   = IC.Nombre,
                       anio     = IE.Anio,
                       fecha    = IE.Fecha,
                       lugar    = IE.Lugar,
                       vrInsc   = IE.VrInscEvent,
                       activo   = IE.Activo
                   }; // Los 10 <th></th> para la tabla
        }

        //Agregar: retorna 0 = error de ejecución, -1 = ya existe el registro,
        //o el código (clave primaria) del registro ingresado
        public int Agregar()
        {
            try
            {
                // Validar si existe
                var existe = ctx.TblEventos.FirstOrDefault(x => x.Nombre == tblEvent.Nombre);
                if (existe != null)
                {
                    return -1; // Ya existe
                }
                // Hallar el código
                int newCod = ctx.TblEventos.Any() ? ctx.TblEventos.Max(x => x.Codigo) : 0;
                tblEvent.Codigo = (newCod <= 0) ? 1 : newCod + 1;
                ctx.TblEventos.Add(tblEvent);
                ctx.SaveChanges();
                return tblEvent.Codigo;
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
                var rgro = ctx.TblEventos.FirstOrDefault(x => x.Codigo == tblEvent.Codigo);
                if (rgro == null)
                {
                    return -1; // Error, no existe
                }
                // Actualiza cada dato
                rgro.Nombre = tblEvent.Nombre;
                rgro.IdCiudad = tblEvent.IdCiudad;
                rgro.Anio = tblEvent.Anio;
                rgro.Fecha = tblEvent.Fecha;
                rgro.Lugar = tblEvent.Lugar;
                rgro.VrInscEvent = tblEvent.VrInscEvent;
                rgro.Activo = tblEvent.Activo;
                ctx.SaveChanges();
                return tblEvent.Codigo;
            }
            catch
            {
                return 0; // Error de ejecución
            }
        }
    }
}
