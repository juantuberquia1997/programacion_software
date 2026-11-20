using apiFestRock.Models;
using static apiFestRock.clases.auxiliar.clsInscripcionCompleta;

namespace apiFestRock.clases
{
    public class clsOpeDetInscrip
    {
        private readonly BdEventosContext ctx;

        public clsOpeDetInscrip(BdEventosContext cntxt)
        {
            ctx = cntxt;
        }

        //Propiedad con los datos del detalle de la inscripción (guía 1.5.3.v)
        public TblDetInsc tbIDetInsc { get; set; }

        //iqueryable: buscar el detalle (artistas inscritos) de una inscripción
        //por su nro. de inscripción (guía 1.5.3.vi)
        public IQueryable buscarDetXNroInsc(int nroInsc)
        {
            return from det in ctx.TblDetInscs
                   join baAr in ctx.TblArtBands on det.IdArtBand equals baAr.Codigo
                   join art in ctx.TblArtista on baAr.IdArtista equals art.Codigo
                   join ins in ctx.TblInstrums on baAr.IdInstrum equals ins.Codigo
                   join ban in ctx.TblBanda on baAr.IdBanda equals ban.Codigo
                   where det.IdNroInsc == nroInsc
                   orderby det.Codigo
                   select new {
                       nroDetInsc = det.Codigo,
                       banda = ban.Nombre,
                       idArtBand = baAr.Codigo,
                       artista = art.Nombre,
                       edad = det.Edad,
                       instrumento = ins.Nombre,
                       activo = baAr.Activo,
                        Eliminar = "<button class='btn btn-info btn-sm btnEliminar'><i class='fas fa-trash-alt'></i> Quitar </button>"
                    };
        }

        //Grabar el detalle (artistas inscritos) de una inscripción: retorna
        //true si grabó todos los registros, false en caso de error (guía 1.7.3.ii)
        public bool registrarDetalles(int idNroInsc, List<detalleInsc> inscritos)
        {
            try
            {
                foreach (var insc in inscritos)
                {
                    TblDetInsc tblDetInsc = new TblDetInsc();
                    tblDetInsc.IdNroInsc = idNroInsc;
                    tblDetInsc.IdArtBand = insc.IdArtBand;
                    tblDetInsc.Edad = insc.Edad;
                    ctx.TblDetInscs.Add(tblDetInsc);
                }
                ctx.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Grabar un artista individual en una inscripción existente:
        //retorna 1 = grabado, 0 = error (guía 1.7.3.iii)
        public int grabarDetalleInd(int idNroInsc, int idArtBanda, int edad)
        {
            try
            {
                TblDetInsc tblDetInsc = new TblDetInsc();
                tblDetInsc.IdNroInsc = idNroInsc;
                tblDetInsc.IdArtBand = idArtBanda;
                tblDetInsc.Edad = edad;

                ctx.TblDetInscs.Add(tblDetInsc);
                ctx.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        //Eliminar el registro (artista) del detalle de una inscripción
        //(guía 1.8.1)
        public string Eliminar(int idDetInsc)
        {
            try
            {
                var det = ctx.TblDetInscs.FirstOrDefault(d => d.Codigo == idDetInsc);
                if (det == null)
                    return "Inscripción no encontrada.";

                ctx.TblDetInscs.Remove(det);
                ctx.SaveChanges();
                return "Registro de Inscripción eliminado correctamente.";
            }
            catch (Exception ex)
            {
                return "Error al eliminar registro: " + ex.Message;
            }
        }
    }
}
