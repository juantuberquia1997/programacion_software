using Microsoft.AspNetCore.Mvc;
using apiFestRock.Models;
using apiFestRock.clases;


namespace apiFestRock.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class departamentoController : ControllerBase
    {

        private readonly BdEventosContext ctx;

        public departamentoController(BdEventosContext ctx)
        {
            this.ctx = ctx;
        }

        [HttpGet("listadoDptos")]
        public List<TblDpto> listarDptos()
        {
            clsOpeDepto x = new clsOpeDepto(ctx);
            return x.listaDptos();
        }
    }
}
