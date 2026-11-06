using Microsoft.AspNetCore.Mvc;
using apiFestRock.Models;
using apiFestRock.clases;

namespace apiFestRock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class instrumController : ControllerBase
    {
        private readonly BdEventosContext ctx;

        public instrumController(BdEventosContext ctx)
        {
            this.ctx = ctx;
        }

        //Combo: instrumentos activos (cboInstrum de frmBanda y frmInscrip)
        [HttpGet("listadoInstrums")]
        public List<TblInstrum> listarInstrums()
        {
            clsOpeInstrum x = new clsOpeInstrum(ctx);
            return x.listaInstrums();
        }

        //Servicio de consulta: comando 1 = consultar por nombre, comando 2 = llenar tabla
        [HttpGet("consultaInstrum")]
        public IQueryable Get(int comando, string dato)
        {
            clsOpeInstrum oX = new clsOpeInstrum(ctx);
            switch (comando)
            {
                case 1: // Consultar un instrumento por nombre
                    return oX.instrumXNombre(dato);
                case 2: // Llenar tabla
                    return oX.listarInstrumsTabla();
                default:
                    return Enumerable.Empty<string>().AsQueryable();
            }
        }

        //Servicio para agregar un registro
        [HttpPost]
        public int Post([FromBody] TblInstrum datIn)
        {
            clsOpeInstrum oX = new clsOpeInstrum(ctx);
            oX.tblInstr = datIn;
            return oX.Agregar();
        }

        //Servicio para modificar un registro
        [HttpPut]
        public int Put([FromBody] TblInstrum datIn)
        {
            clsOpeInstrum oX = new clsOpeInstrum(ctx);
            oX.tblInstr = datIn;
            return oX.Modificar();
        }
    }
}
