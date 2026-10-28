using Microsoft.AspNetCore.Mvc;
using apiFestRock.Models;
using apiFestRock.clases;


namespace apiFestRock.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class tipoDocController : ControllerBase
    {

        private readonly BdEventosContext ctx;

        public tipoDocController(BdEventosContext ctx)
        {
            this.ctx = ctx;
        }

        [HttpGet("listadoTipDocs")]
        public List<TblTipoDoc> listarTiposDoc()
        {
            clsOpeTipoDoc x = new clsOpeTipoDoc(ctx);
            return x.listaTipDocs();
        }
    }
}
