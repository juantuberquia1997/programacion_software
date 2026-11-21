using apiLavanderia.Models;

namespace apiLavanderia.clases
{
    public class clsOpeEstadoPrenda
    {
        private readonly LavanderiaContext ctx;

        public clsOpeEstadoPrenda(LavanderiaContext ctx)
        {
            this.ctx = ctx;
        }

        //Propiedad de entrada y salida con los datos del estado de la prenda
        public TblEstadoPrenda tblEstadoPrenda { get; set; }

        //Combo: estados de la prenda
        public List<TblEstadoPrenda> listaEstadoPrendas()
        {
            return ctx.TblEstadoPrendas
                .OrderBy(x => x.EstadoPrenda)
                .ToList();
        }

        //iqueryable: consultar un estado de prenda por su nombre
        public IQueryable estadoPrendaXNombre(string nombre)
        {
            return from IE in ctx.Set<TblEstadoPrenda>()
                   where IE.EstadoPrenda == nombre
                   select new {
                       codigo = IE.IdEstadoPrenda,
                       nombre = IE.EstadoPrenda
                   };
        }

        //iqueryable: listar todos los estados de prenda para llenar la tabla
        public IQueryable listarEstadoPrendasTabla()
        {
            return from IE in ctx.Set<TblEstadoPrenda>()
                   orderby IE.IdEstadoPrenda
                   select new {
                       editar = "<a class='btn btn-info btn-sm' href=''><i class='fa fa-search-plus'></i> ir </a>",
                       codigo = IE.IdEstadoPrenda,
                       nombre = IE.EstadoPrenda
                   }; // Los 3 <th></th> para la tabla
        }

        //Agregar: retorna 0 = error de ejecucion, -1 = ya existe el registro,
        //o el codigo (clave primaria) del registro ingresado
        public int Agregar()
        {
            try
            {
                // Validar si existe
                var existe = ctx.TblEstadoPrendas.FirstOrDefault(x => x.EstadoPrenda == tblEstadoPrenda.EstadoPrenda);
                if (existe != null)
                {
                    return -1; // Ya existe
                }
                // Hallar el codigo
                int newCod = ctx.TblEstadoPrendas.Any() ? ctx.TblEstadoPrendas.Max(x => x.IdEstadoPrenda) : 0;
                tblEstadoPrenda.IdEstadoPrenda = (newCod <= 0) ? 1 : newCod + 1;
                ctx.TblEstadoPrendas.Add(tblEstadoPrenda);
                ctx.SaveChanges();
                return tblEstadoPrenda.IdEstadoPrenda;
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
                var rgro = ctx.TblEstadoPrendas.FirstOrDefault(x => x.IdEstadoPrenda == tblEstadoPrenda.IdEstadoPrenda);
                if (rgro == null)
                {
                    return -1; // Error, no existe
                }
                // Actualiza cada dato
                rgro.EstadoPrenda = tblEstadoPrenda.EstadoPrenda;
                ctx.SaveChanges();
                return tblEstadoPrenda.IdEstadoPrenda;
            }
            catch
            {
                return 0; // Error de ejecucion
            }
        }
    }
}
