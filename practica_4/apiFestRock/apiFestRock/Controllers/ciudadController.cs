using Microsoft.AspNetCore.Mvc;
using apiFestRock.Models;
using apiFestRock.clases;


namespace apiFestRock.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ciudadController : ControllerBase
    {

        private readonly BdEventosContext ctx;

        public ciudadController(BdEventosContext ctx)
        {
            this.ctx = ctx;
        }

        [HttpGet("listadoCiudsXDpto")]
        public List<TblCiudad> listarCiudadesXDpto(int codDpto)
        {
            clsOpeCiudad x = new clsOpeCiudad(ctx);
            return x.listaCiudadesxDpto(codDpto);
        }
    }
}
