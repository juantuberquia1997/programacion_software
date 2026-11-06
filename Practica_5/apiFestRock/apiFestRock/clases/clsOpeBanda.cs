using apiFestRock.Models;

namespace apiFestRock.clases
{
    //Clase de entrada para guardar la banda con su detalle de artistas (tblArtBand)
    public class clsBandaIn
    {
        public TblBandum tblBand { get; set; } = new TblBandum();
        public List<TblArtBand> detalle { get; set; } = new List<TblArtBand>();
    }

    public class clsOpeBanda
    {
        private readonly BdEventosContext ctx;

        public clsOpeBanda(BdEventosContext ctx)
        {
            this.ctx = ctx;
        }

        //Combo: bandas activas (cboBanda del form Inscripción)
        public List<TblBandum> listaBandas()
        {
            return ctx.TblBanda
                .Where(x => x.Activo == true)
                .OrderBy(x => x.Nombre)
                .ToList();
        }

        //iqueryable: consultar una banda por su nombre
        public IQueryable bandaXNombre(string nombre)
        {
            return from IB in ctx.Set<TblBandum>()
                   join IC in ctx.Set<TblCiudad>() on IB.IdCiudad equals IC.Codigo
                   where IB.Nombre == nombre
                   select new {
                       codigo   = IB.Codigo,
                       nombre   = IB.Nombre,
                       anioFund = IB.AnioFund,
                       idDep    = IC.IdDpto,
                       idC      = IB.IdCiudad,
                       represent = IB.Represent,
                       tel      = IB.Telefono,
                       act      = IB.Activo
                   };
        }

        //iqueryable: listar todas las bandas para llenar la tabla
        public IQueryable listarBandasTabla()
        {
            return from IB in ctx.Set<TblBandum>()
                   join IC in ctx.Set<TblCiudad>() on IB.IdCiudad equals IC.Codigo
                   join ID in ctx.Set<TblDpto>() on IC.IdDpto equals ID.Codigo
                   orderby IB.Nombre
                   select new {
                       editar   = "<a class='btn btn-info btn-sm' href=''><i class='fa fa-search-plus'></i> ir </a>",
                       codigo   = IB.Codigo,
                       nombre   = IB.Nombre,
                       anio     = IB.AnioFund,
                       dpto     = ID.Nombre,
                       ciudad   = IC.Nombre,
                       represent = IB.Represent,
                       activo   = IB.Activo
                   }; // Los 8 <th></th> para la tabla
        }

        //iqueryable: artistas de una banda con su instrumento
        //(detalle del form Banda, cboArtBand del form Inscripción y consulta Artista por banda)
        public IQueryable artistasXBanda(int codBanda)
        {
            return from AB in ctx.Set<TblArtBand>()
                   join IA in ctx.Set<TblArtistum>() on AB.IdArtista equals IA.Codigo
                   join II in ctx.Set<TblInstrum>() on AB.IdInstrum equals II.Codigo
                   where AB.IdBanda == codBanda
                   orderby IA.Nombre
                   select new {
                       idArtBand   = AB.Codigo,
                       idArtista   = IA.Codigo,
                       artista     = IA.Nombre,
                       idInstrum   = II.Codigo,
                       instrumento = II.Nombre,
                       activo      = AB.Activo
                   };
        }

        //Agregar banda con su detalle de artistas (una sola transacción):
        //retorna 0 = error de ejecución, -1 = ya existe el registro,
        //o el código (clave primaria) del registro ingresado
        public int Agregar(clsBandaIn datIn)
        {
            try
            {
                TblBandum tblBand = datIn.tblBand;
                // Validar si existe
                var existe = ctx.TblBanda.FirstOrDefault(x => x.Nombre == tblBand.Nombre);
                if (existe != null)
                {
                    return -1; // Ya existe
                }
                // Hallar el código
                int newCod = ctx.TblBanda.Any() ? ctx.TblBanda.Max(x => x.Codigo) : 0;
                tblBand.Codigo = (newCod <= 0) ? 1 : newCod + 1;
                // Transacción: primero el encabezado, luego el detalle
                //(el modelo no tiene las relaciones, EF no garantiza el orden entre tablas)
                using (var transaccion = ctx.Database.BeginTransaction())
                {
                    ctx.TblBanda.Add(tblBand);
                    ctx.SaveChanges();
                    foreach (var art in datIn.detalle)
                    {
                        art.Codigo = 0; // Identity: lo asigna la base de datos
                        art.IdBanda = tblBand.Codigo;
                        ctx.TblArtBands.Add(art);
                    }
                    ctx.SaveChanges();
                    transaccion.Commit();
                }
                return tblBand.Codigo;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error Agregar banda: " + ex.Message);
                return 0; // Error de ejecución
            }
        }

        //Modificar banda y actualizar su detalle de artistas:
        //retorna 0 = error de ejecución, -1 = no existe el registro,
        //o el código (clave primaria) del registro modificado
        public int Modificar(clsBandaIn datIn)
        {
            try
            {
                TblBandum tblBand = datIn.tblBand;
                // Validar si existe
                var rgro = ctx.TblBanda.FirstOrDefault(x => x.Codigo == tblBand.Codigo);
                if (rgro == null)
                {
                    return -1; // Error, no existe
                }
                // Actualiza cada dato
                rgro.Nombre = tblBand.Nombre;
                rgro.AnioFund = tblBand.AnioFund;
                rgro.Represent = tblBand.Represent;
                rgro.Telefono = tblBand.Telefono;
                rgro.IdCiudad = tblBand.IdCiudad;
                rgro.Activo = tblBand.Activo;
                // Actualizar el detalle de artistas por banda: no se eliminan registros
                //(pueden tener inscripciones asociadas), se actualizan o se adicionan
                foreach (var art in datIn.detalle)
                {
                    if (art.Codigo > 0)
                    {
                        var rgroArt = ctx.TblArtBands.FirstOrDefault(x => x.Codigo == art.Codigo && x.IdBanda == rgro.Codigo);
                        if (rgroArt != null)
                        {
                            rgroArt.IdArtista = art.IdArtista;
                            rgroArt.IdInstrum = art.IdInstrum;
                            rgroArt.Activo = art.Activo;
                        }
                    }
                    else
                    {
                        art.Codigo = 0; // Identity: lo asigna la base de datos
                        art.IdBanda = rgro.Codigo;
                        ctx.TblArtBands.Add(art);
                    }
                }
                ctx.SaveChanges();
                return rgro.Codigo;
            }
            catch
            {
                return 0; // Error de ejecución
            }
        }
    }
}
