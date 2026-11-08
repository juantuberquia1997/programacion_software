using Microsoft.AspNetCore.Mvc;
using apiFestRock.Models;
using apiFestRock.clases;

namespace apiFestRock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class inscripcController : ControllerBase
    {
        private readonly BdEventosContext ctx;

        public inscripcController(BdEventosContext context)
        {
            ctx = context;
        }

        //Servicio de consulta del encabezado de una inscripción por su nro.
        //(guía 1.5.2.vi)
        [HttpGet("buscarInscrip")]
        public IQueryable Get(int nroInsc)
        {
            clsOpeInscrip oX = new clsOpeInscrip(ctx);
            return oX.consultarInscrip(nroInsc);
        }
    }
}
