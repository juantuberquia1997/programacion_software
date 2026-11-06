using Microsoft.AspNetCore.Mvc;
using apiFestRock.Models;
using apiFestRock.clases;

namespace apiFestRock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class consultaController : ControllerBase
    {
        private readonly BdEventosContext ctx;

        public consultaController(BdEventosContext ctx)
        {
            this.ctx = ctx;
        }

        //Consultas por evento (form Artista por banda)

        //Bandas participantes de un evento
        [HttpGet("bandasXEvento")]
        public IQueryable GetBandasXEvento(int idEvento)
        {
            clsOpeConsulta x = new clsOpeConsulta(ctx);
            return x.bandasXEvento(idEvento);
        }

        //Artistas de una banda inscritos en un evento (con su edad)
        [HttpGet("artistasXBandaEvento")]
        public IQueryable GetArtistasXBandaEvento(int idEvento, int codBanda)
        {
            clsOpeConsulta x = new clsOpeConsulta(ctx);
            return x.artistasXBandaEvento(idEvento, codBanda);
        }

        //Artistas por instrumento de un evento
        [HttpGet("artistasXInstrum")]
        public IQueryable GetArtistasXInstrum(int idEvento, int idInstrum)
        {
            clsOpeConsulta x = new clsOpeConsulta(ctx);
            return x.artistasXInstrum(idEvento, idInstrum);
        }
    }
}
