using Microsoft.AspNetCore.Mvc;
using apiFestRock.Models;
using apiFestRock.clases;


namespace apiFestRock.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class generoController : ControllerBase
    {

        private readonly BdEventosContext ctx;

        public generoController(BdEventosContext ctx)
        {
            this.ctx = ctx;
        }
        //nombre del servicio listaGeneros
        [HttpGet("listaGeneros")]
        public List<TblGenero> listarGeneros() {
            clsOpeGenero x = new clsOpeGenero(ctx);
            return x.listaGeneros();
        }



    }
}
