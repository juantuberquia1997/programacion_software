using Microsoft.AspNetCore.Mvc;
using apiFestRock.Models;
using apiFestRock.clases;

namespace apiFestRock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class artBandController : ControllerBase
    {
        private readonly BdEventosContext ctx;

        public artBandController(BdEventosContext context)
        {
            ctx = context;
        }

        //Servicio de consulta: cmd 1 = llenar combo de los artistas por banda,
        //cmd 2 = consultar el nombre del instrumento de un artista
        [HttpGet("datosArtBanda")]
        public IQueryable infoArtBand(int dato, int cmd)
        {
            clsOpeArtBand oX = new clsOpeArtBand(ctx);
            switch (cmd)
            {
                case 1: // Llenar combo de los artistas por banda
                    return oX.listarArtistasXBand(dato);
                case 2: // Consultar el nombre del instrumento de un artista
                    return oX.consultaInstrumArtBand(dato);
                default:
                    return Enumerable.Empty<string>().AsQueryable();
            }
        }
    }
}
