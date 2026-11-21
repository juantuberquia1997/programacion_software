using apiFestRock.Models;

namespace apiFestRock.clases
{
    public class clsOpeArtBand
    {
        private readonly BdEventosContext ctx;

        public clsOpeArtBand(BdEventosContext cntxt)
        {
            ctx = cntxt;
        }

        //iqueryable: llenar el combo de los artistas de una banda (cboArtBand),
        //solo los que se encuentran activos
        public IQueryable listarArtistasXBand(int idB)
        {
            return from tBA in ctx.Set<TblArtBand>()
                   join tA in ctx.Set<TblArtistum>() on tBA.IdArtista equals tA.Codigo
                   where tBA.IdBanda == idB && tBA.Activo == true
                   select new {
                       codigo = tBA.Codigo,
                       nombre = tA.Nombre
                   };
        }

        //iqueryable: consultar el nombre del instrumento que toca un artista
        //de una banda, acorde al código del artista por banda (txtInstrum)
        public IQueryable consultaInstrumArtBand(int idInscrip)
        {
            return from tBA in ctx.Set<TblArtBand>()
                   join tI in ctx.Set<TblInstrum>() on tBA.IdInstrum equals tI.Codigo
                   where tBA.Codigo == idInscrip
                   select new {
                       nombre = tI.Nombre
                   };
        }
    }
}
