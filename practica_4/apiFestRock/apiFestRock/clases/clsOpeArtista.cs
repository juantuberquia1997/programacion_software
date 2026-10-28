using apiFestRock.Models;

namespace apiFestRock.clases
{
    public class clsOpeArtista
    {
        private readonly BdEventosContext ctx;

        public clsOpeArtista(BdEventosContext ctx)
        {
            this.ctx = ctx;
        }

        public TblArtistum tblArtis { get; set; }

        //iqueryable: consultar un artista por nro de documento
        //
        public IQueryable artistaXNroDoc(int nroDocu)
        {
            //tartist y tCity son alias
            return from tArtist in ctx.Set<TblArtistum>()
                   join tCity in ctx.Set<TblCiudad>() on tArtist.IdCiudad equals tCity.Codigo
                   where tArtist.NroDoc == nroDocu
                   select new {
                       codigo = tArtist.Codigo,
                       nombre = tArtist.Nombre,
                       idTD   = tArtist.IdtipoDoc,
                       nroDoc = tArtist.NroDoc,
                       idG    = tArtist.IdGenero,
                       idDep  = tCity.IdDpto,
                       idC    = tArtist.IdCiudad,
                       act    = tArtist.Activo
                   };
        }

        //iqueryable: listar todos los artistas para llenar la tabla
        public IQueryable listarArtistas()
        {
            return from tArtist in ctx.Set<TblArtistum>()
                   join ITd in ctx.Set<TblTipoDoc>() on tArtist.IdtipoDoc equals ITd.Codigo
                   join IG  in ctx.Set<TblGenero>()  on tArtist.IdGenero  equals IG.Codigo
                   join tCity  in ctx.Set<TblCiudad>()  on tArtist.IdCiudad  equals tCity.Codigo
                   join ID  in ctx.Set<TblDpto>()    on tCity.IdDpto    equals ID.Codigo
                   orderby tArtist.NroDoc
                   select new {
                       //Es doble comillas simples
                       editar   = "<a class='btn btn-info btn-sm' href=''><i class='fa fa-search-plus'></i> ir </a>",
                       codigo   = tArtist.Codigo,
                       nombre   = tArtist.Nombre,
                       tipo_Doc = tArtist.IdtipoDoc,
                       nro_Doc  = tArtist.NroDoc,
                       genero   = tArtist.IdGenero,
                       dpto     = tCity.IdDpto,
                       ciudad   = tCity.Nombre,
                       activo   = tArtist.Activo
                    }; // Los 9 <th></th> para la tabla
        }

        //Agregar: retorna 0 = error de ejecución, -1 = ya existe el registro,
        //o el código (clave primaria) del registro ingresado
        public int Agregar()
        {
            try
            {
                // Validar si existe
                var existe = ctx.TblArtista.FirstOrDefault(x => x.NroDoc == tblArtis.NroDoc);
                if (existe != null)
                {
                    return -1; // Ya existe
                }
                else
                { // Hallar el código
                    int newCod = ctx.TblArtista.Max(x => x.Codigo);
                    tblArtis.Codigo = (newCod <= 0) ? 1 : newCod + 1;
                    ctx.TblArtista.Add(tblArtis);
                    ctx.SaveChangesAsync();
                    return tblArtis.Codigo; // Código como éxito
                }
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
                var rgro = ctx.TblArtista.FirstOrDefault(x => x.Codigo == tblArtis.Codigo);
                if (rgro == null)
                {
                    return -1; // Error, no existe
                }
                // Actualiza cada dato
                rgro.Nombre = tblArtis.Nombre;
                rgro.IdtipoDoc = tblArtis.IdtipoDoc;
                rgro.NroDoc = tblArtis.NroDoc;
                rgro.IdGenero = tblArtis.IdGenero;
                rgro.IdCiudad = tblArtis.IdCiudad;
                rgro.Activo = tblArtis.Activo;
                ctx.SaveChanges();
                return tblArtis.Codigo;
            }
            catch
            {
                return 0; // Error de ejecución
            }
        }
    }
}
