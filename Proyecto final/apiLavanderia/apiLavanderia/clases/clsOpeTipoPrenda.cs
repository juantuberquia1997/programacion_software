using apiLavanderia.Models;

namespace apiLavanderia.clases
{
    public class clsOpeTipoPrenda
    {
        private readonly LavanderiaContext ctx;

        public clsOpeTipoPrenda(LavanderiaContext ctx)
        {
            this.ctx = ctx;
        }

        //Propiedad de entrada y salida con los datos del tipo de prenda
        public TblTipoPrenda tblTipoPrenda { get; set; }

        //Combo: tipos de prenda
        public List<TblTipoPrenda> listaTipoPrendas()
        {
            return ctx.TblTipoPrendas
                .OrderBy(x => x.NombrePrenda)
                .ToList();
        }

        //iqueryable: consultar un tipo de prenda por su nombre
        public IQueryable tipoPrendaXNombre(string nombre)
        {
            return from IP in ctx.Set<TblTipoPrenda>()
                   where IP.NombrePrenda == nombre
                   select new {
                       codigo = IP.IdTipoPrenda,
                       nombre = IP.NombrePrenda,
                       activo = IP.Activo
                   };
        }

        //iqueryable: listar todos los tipos de prenda para llenar la tabla
        public IQueryable listarTipoPrendasTabla()
        {
            return from IP in ctx.Set<TblTipoPrenda>()
                   orderby IP.NombrePrenda
                   select new {
                       editar = "<a class='btn btn-info btn-sm' href=''><i class='fa fa-search-plus'></i> ir </a>",
                       codigo = IP.IdTipoPrenda,
                       nombre = IP.NombrePrenda,
                       activo = IP.Activo
                   }; // Los 3 <th></th> para la tabla
        }

        //Agregar: retorna 0 = error de ejecucion, -1 = ya existe el registro,
        //o el codigo (clave primaria) del registro ingresado
        public int Agregar()
        {
            try
            {
                // Validar si existe
                var existe = ctx.TblTipoPrendas.FirstOrDefault(x => x.NombrePrenda == tblTipoPrenda.NombrePrenda);
                if (existe != null)
                {
                    return -1; // Ya existe
                }
                // Hallar el codigo
                int newCod = ctx.TblTipoPrendas.Any() ? ctx.TblTipoPrendas.Max(x => x.IdTipoPrenda) : 0;
                tblTipoPrenda.IdTipoPrenda = (newCod <= 0) ? 1 : newCod + 1;
                ctx.TblTipoPrendas.Add(tblTipoPrenda);
                ctx.SaveChanges();
                return tblTipoPrenda.IdTipoPrenda;
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
                var rgro = ctx.TblTipoPrendas.FirstOrDefault(x => x.IdTipoPrenda == tblTipoPrenda.IdTipoPrenda);
                if (rgro == null)
                {
                    return -1; // Error, no existe
                }
                // Actualiza cada dato
                rgro.NombrePrenda = tblTipoPrenda.NombrePrenda;
                ctx.SaveChanges();
                return tblTipoPrenda.IdTipoPrenda;
            }
            catch
            {
                return 0; // Error de ejecucion
            }
        }
    }
}
