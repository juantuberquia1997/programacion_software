using apiLavanderia.Models;

namespace apiLavanderia.clases
{
    public class clsOpeDepartamento
    {
        private readonly LavanderiaContext ctx;

        public clsOpeDepartamento(LavanderiaContext ctx)
        {
            this.ctx = ctx;
        }

        //Propiedad de entrada y salida con los datos del departamento
        public TblDepartamento tblDepartamento { get; set; }

        //Combo: departamentos
        public List<TblDepartamento> listaDepartamentos()
        {
            return ctx.TblDepartamentos
                .OrderBy(x => x.NombreDepartamento)
                .ToList();
        }

        //iqueryable: consultar un departamento por su nombre
        public IQueryable departamentoXNombre(string nombre)
        {
            return from ID in ctx.Set<TblDepartamento>()
                   where ID.NombreDepartamento == nombre
                   select new {
                       codigo = ID.IdDepartamento,
                       nombre = ID.NombreDepartamento
                   };
        }

        //iqueryable: listar todos los departamentos para llenar la tabla
        public IQueryable listarDepartamentosTabla()
        {
            return from ID in ctx.Set<TblDepartamento>()
                   orderby ID.NombreDepartamento
                   select new {
                       editar = "<a class='btn btn-info btn-sm' href=''><i class='fa fa-search-plus'></i> ir </a>",
                       codigo = ID.IdDepartamento,
                       nombre = ID.NombreDepartamento
                   }; // Los 3 <th></th> para la tabla
        }

        //Agregar: retorna 0 = error de ejecucion, -1 = ya existe el registro,
        //o el codigo (clave primaria) del registro ingresado
        public int Agregar()
        {
            try
            {
                // Validar si existe
                var existe = ctx.TblDepartamentos.FirstOrDefault(x => x.NombreDepartamento == tblDepartamento.NombreDepartamento);
                if (existe != null)
                {
                    return -1; // Ya existe
                }
                // Hallar el codigo
                int newCod = ctx.TblDepartamentos.Any() ? ctx.TblDepartamentos.Max(x => x.IdDepartamento) : 0;
                tblDepartamento.IdDepartamento = (newCod <= 0) ? 1 : newCod + 1;
                ctx.TblDepartamentos.Add(tblDepartamento);
                ctx.SaveChanges();
                return tblDepartamento.IdDepartamento;
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
                var rgro = ctx.TblDepartamentos.FirstOrDefault(x => x.IdDepartamento == tblDepartamento.IdDepartamento);
                if (rgro == null)
                {
                    return -1; // Error, no existe
                }
                // Actualiza cada dato
                rgro.NombreDepartamento = tblDepartamento.NombreDepartamento;
                ctx.SaveChanges();
                return tblDepartamento.IdDepartamento;
            }
            catch
            {
                return 0; // Error de ejecucion
            }
        }
    }
}
