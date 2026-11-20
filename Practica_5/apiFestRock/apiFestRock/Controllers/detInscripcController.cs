using Microsoft.AspNetCore.Mvc;
using apiFestRock.Models;
using apiFestRock.clases;

namespace apiFestRock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class detInscripcController : ControllerBase
    {
        private readonly BdEventosContext ctx;

        public detInscripcController(BdEventosContext context)
        {
            ctx = context;
        }

        //Servicio de consulta del detalle de una inscripción por su nro.
        //(guía 1.5.4.vi)
        [HttpGet("consultaDatosDetInscrip")]
        public IQueryable Get(int nroInsc)
        {
            clsOpeDetInscrip oX = new clsOpeDetInscrip(ctx);
            return oX.buscarDetXNroInsc(nroInsc);
        }

        //Grabar un artista individual en una inscripción existente:
        //retorna 1 = grabado, 0 = error (guía 1.7.5)
        [HttpPost("agregarDetalleInd")]
        public int adicionaDetalleInd(int nroInscrip, int idArtBand, int edad)
        {
            clsOpeDetInscrip oX = new clsOpeDetInscrip(ctx);
            return oX.grabarDetalleInd(nroInscrip, idArtBand, edad);
        }

        //Eliminar el registro (artista) del detalle de una inscripción
        //(guía 1.8.2)
        [HttpDelete("borrarDetalle")]
        public string borradoDetalle(int idDet)
        {
            clsOpeDetInscrip oX = new clsOpeDetInscrip(ctx);
            return oX.Eliminar(idDet);
        }
    }
}
