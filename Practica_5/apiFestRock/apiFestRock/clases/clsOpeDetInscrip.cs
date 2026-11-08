using apiFestRock.Models;

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
    }
}
