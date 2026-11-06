using apiFestRock.Models;

namespace apiFestRock.clases
{
    //Clase de entrada para guardar la inscripción con su detalle de artistas (tblDetInsc)
    public class clsInscripIn
    {
        public TblInscrip tblInsc { get; set; } = new TblInscrip();
        public List<TblDetInsc> detalle { get; set; } = new List<TblDetInsc>();
    }

    public class clsOpeInscrip
    {
        private readonly BdEventosContext ctx;

        public clsOpeInscrip(BdEventosContext ctx)
        {
            this.ctx = ctx;
        }

        //iqueryable: consultar una inscripción por su código (nro. de inscripción)
        public IQueryable inscripXId(int codigo)
        {
            return from IN in ctx.Set<TblInscrip>()
                   join IE in ctx.Set<TblEvento>() on IN.IdEvento equals IE.Codigo
                   join IB in ctx.Set<TblBandum>() on IN.IdBanda equals IB.Codigo
                   where IN.Codigo == codigo
                   select new {
                       codigo  = IN.Codigo,
                       idEvento = IN.IdEvento,
                       evento  = IE.Nombre,
                       fecha   = IN.Fecha,
                       idBanda = IN.IdBanda,
                       banda   = IB.Nombre,
                       represent = IN.Represent,
                       tel      = IN.TelRepresent,
                       valor    = IN.ValorInscrip
                   };
        }

        //iqueryable: consultar una inscripción por evento y banda
        public IQueryable inscripXEventoBanda(int idEvento, int idBanda)
        {
            return from IN in ctx.Set<TblInscrip>()
                   join IE in ctx.Set<TblEvento>() on IN.IdEvento equals IE.Codigo
                   join IB in ctx.Set<TblBandum>() on IN.IdBanda equals IB.Codigo
                   where IN.IdEvento == idEvento && IN.IdBanda == idBanda
                   select new {
                       codigo  = IN.Codigo,
                       idEvento = IN.IdEvento,
                       evento  = IE.Nombre,
                       fecha   = IN.Fecha,
                       idBanda = IN.IdBanda,
                       banda   = IB.Nombre,
                       represent = IN.Represent,
                       tel      = IN.TelRepresent,
                       valor    = IN.ValorInscrip
                   };
        }

        //iqueryable: listar todas las inscripciones para llenar la tabla
        public IQueryable listarInscripsTabla()
        {
            return from IN in ctx.Set<TblInscrip>()
                   join IE in ctx.Set<TblEvento>() on IN.IdEvento equals IE.Codigo
                   join IB in ctx.Set<TblBandum>() on IN.IdBanda equals IB.Codigo
                   orderby IN.Codigo
                   select new {
                       editar   = "<a class='btn btn-info btn-sm' href=''><i class='fa fa-search-plus'></i> ir </a>",
                       nroInsc  = IN.Codigo,
                       evento   = IE.Nombre,
                       fecha    = IN.Fecha,
                       banda    = IB.Nombre,
                       represent = IN.Represent,
                       tel      = IN.TelRepresent,
                       valor    = IN.ValorInscrip
                   }; // Los 8 <th></th> para la tabla
        }

        //iqueryable: detalle de una inscripción (artistas inscritos con su edad)
        public IQueryable detalleInsc(int idNroInsc)
        {
            return from DI in ctx.Set<TblDetInsc>()
                   join AB in ctx.Set<TblArtBand>() on DI.IdArtBand equals AB.Codigo
                   join IA in ctx.Set<TblArtistum>() on AB.IdArtista equals IA.Codigo
                   join II in ctx.Set<TblInstrum>() on AB.IdInstrum equals II.Codigo
                   where DI.IdNroInsc == idNroInsc
                   orderby IA.Nombre
                   select new {
                       idArtBand   = AB.Codigo,
                       idArtista   = IA.Codigo,
                       artista     = IA.Nombre,
                       idInstrum   = II.Codigo,
                       instrumento = II.Nombre,
                       edad        = DI.Edad
                   };
        }

        //Agregar inscripción con su detalle de artistas (una sola transacción):
        //retorna 0 = error de ejecución, -1 = la banda ya está inscrita en el evento,
        //-2 = el detalle está vacío (mínimo 1 artista),
        //o el código (clave primaria) del registro ingresado
        public int Agregar(clsInscripIn datIn)
        {
            try
            {
                TblInscrip tblInsc = datIn.tblInsc;
                // Validar si la banda ya está inscrita en el evento
                var existe = ctx.TblInscrips.FirstOrDefault(x => x.IdEvento == tblInsc.IdEvento && x.IdBanda == tblInsc.IdBanda);
                if (existe != null)
                {
                    return -1; // Ya existe
                }
                // La banda debe inscribir un mínimo de 1 artista (solitario)
                if (datIn.detalle == null || datIn.detalle.Count == 0)
                {
                    return -2; // Detalle vacío
                }
                // Consultar el valor por artista del evento
                var evento = ctx.TblEventos.FirstOrDefault(x => x.Codigo == tblInsc.IdEvento);
                if (evento == null)
                {
                    return 0; // El evento no existe
                }
                // Hallar el código
                int newCod = ctx.TblInscrips.Any() ? ctx.TblInscrips.Max(x => x.Codigo) : 0;
                tblInsc.Codigo = (newCod <= 0) ? 1 : newCod + 1;
                if (string.IsNullOrEmpty(tblInsc.Fecha))
                {
                    tblInsc.Fecha = DateTime.Now.ToString("dd/MM/yyyy");
                }
                // El valor de la inscripción se actualiza en cada registro:
                // valor por artista del evento x nro. de artistas inscritos
                tblInsc.ValorInscrip = (evento.VrInscEvent ?? 0) * datIn.detalle.Count;
                // Transacción: primero el encabezado, luego el detalle
                //(el modelo no tiene las relaciones, EF no garantiza el orden entre tablas)
                using (var transaccion = ctx.Database.BeginTransaction())
                {
                    ctx.TblInscrips.Add(tblInsc);
                    ctx.SaveChanges();
                    foreach (var det in datIn.detalle)
                    {
                        det.Codigo = 0; // Identity: lo asigna la base de datos
                        det.IdNroInsc = tblInsc.Codigo;
                        ctx.TblDetInscs.Add(det);
                    }
                    ctx.SaveChanges();
                    transaccion.Commit();
                }
                return tblInsc.Codigo;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error Agregar inscripción: " + ex.Message);
                return 0; // Error de ejecución
            }
        }

        //Modificar inscripción (encabezado y detalle; el valor no se recalcula):
        //retorna 0 = error de ejecución, -1 = no existe el registro,
        //o el código (clave primaria) del registro modificado
        public int Modificar(clsInscripIn datIn)
        {
            try
            {
                TblInscrip tblInsc = datIn.tblInsc;
                // Validar si existe
                var rgro = ctx.TblInscrips.FirstOrDefault(x => x.Codigo == tblInsc.Codigo);
                if (rgro == null)
                {
                    return -1; // Error, no existe
                }
                // Actualiza cada dato (si no llega la fecha, conserva la del registro)
                if (!string.IsNullOrEmpty(tblInsc.Fecha))
                {
                    rgro.Fecha = tblInsc.Fecha;
                }
                rgro.Represent = tblInsc.Represent;
                rgro.TelRepresent = tblInsc.TelRepresent;
                // Si se retira un artista de una inscripción ya realizada,
                // no se actualiza el valor: se realizará por otro proceso
                // Reemplazar el detalle de la inscripción
                var detAnt = ctx.TblDetInscs.Where(x => x.IdNroInsc == rgro.Codigo).ToList();
                ctx.TblDetInscs.RemoveRange(detAnt);
                foreach (var det in datIn.detalle)
                {
                    det.Codigo = 0; // Identity: lo asigna la base de datos
                    det.IdNroInsc = rgro.Codigo;
                    ctx.TblDetInscs.Add(det);
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
