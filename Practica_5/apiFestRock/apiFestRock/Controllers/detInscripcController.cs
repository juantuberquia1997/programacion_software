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
    }
}
