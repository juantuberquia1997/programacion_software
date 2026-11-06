using apiFestRock.Models;

namespace apiFestRock.clases
{
    //Consultas por evento: bandas participantes, artista por banda
    //y artistas por instrumento (form Artista por banda)
    public class clsOpeConsulta
    {
        private readonly BdEventosContext ctx;

        public clsOpeConsulta(BdEventosContext ctx)
        {
            this.ctx = ctx;
        }

        //iqueryable: bandas participantes de un evento
        public IQueryable bandasXEvento(int idEvento)
        {
            return from IN in ctx.Set<TblInscrip>()
                   join IB in ctx.Set<TblBandum>() on IN.IdBanda equals IB.Codigo
                   join IC in ctx.Set<TblCiudad>() on IB.IdCiudad equals IC.Codigo
                   where IN.IdEvento == idEvento
                   orderby IB.Nombre
                   select new {
                       nroInsc   = IN.Codigo,
                       banda     = IB.Nombre,
                       anio      = IB.AnioFund,
                       ciudad    = IC.Nombre,
                       represent = IN.Represent,
                       tel       = IN.TelRepresent,
                       valor     = IN.ValorInscrip,
                       nroArtistas = ctx.Set<TblDetInsc>().Count(D => D.IdNroInsc == IN.Codigo)
                   };
        }

        //iqueryable: artistas de una banda inscritos en un evento (con su edad)
        public IQueryable artistasXBandaEvento(int idEvento, int codBanda)
        {
            return from IN in ctx.Set<TblInscrip>()
                   join IB in ctx.Set<TblBandum>() on IN.IdBanda equals IB.Codigo
                   join AB in ctx.Set<TblArtBand>() on IN.IdBanda equals AB.IdBanda
                   join IA in ctx.Set<TblArtistum>() on AB.IdArtista equals IA.Codigo
                   join II in ctx.Set<TblInstrum>() on AB.IdInstrum equals II.Codigo
                   join DI in ctx.Set<TblDetInsc>() on new { idNroInsc = IN.Codigo, idArtBand = AB.Codigo }
                       equals new { idNroInsc = DI.IdNroInsc, idArtBand = DI.IdArtBand }
                   where IN.IdEvento == idEvento && AB.IdBanda == codBanda
                   orderby IA.Nombre
                   select new {
                       nroInsc     = IN.Codigo,
                       banda       = IB.Nombre,
                       artista     = IA.Nombre,
                       instrumento = II.Nombre,
                       edad        = DI.Edad
                   };
        }

        //iqueryable: artistas por instrumento de un evento
        public IQueryable artistasXInstrum(int idEvento, int idInstrum)
        {
            return from IN in ctx.Set<TblInscrip>()
                   join IB in ctx.Set<TblBandum>() on IN.IdBanda equals IB.Codigo
                   join AB in ctx.Set<TblArtBand>() on IN.IdBanda equals AB.IdBanda
                   join IA in ctx.Set<TblArtistum>() on AB.IdArtista equals IA.Codigo
                   join II in ctx.Set<TblInstrum>() on AB.IdInstrum equals II.Codigo
                   join DI in ctx.Set<TblDetInsc>() on new { idNroInsc = IN.Codigo, idArtBand = AB.Codigo }
                       equals new { idNroInsc = DI.IdNroInsc, idArtBand = DI.IdArtBand }
                   where IN.IdEvento == idEvento && AB.IdInstrum == idInstrum
                   orderby IB.Nombre, IA.Nombre
                   select new {
                       banda       = IB.Nombre,
                       artista     = IA.Nombre,
                       instrumento = II.Nombre,
                       edad        = DI.Edad
                   };
        }
    }
}
