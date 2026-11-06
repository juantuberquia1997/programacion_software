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

        //Servicio de consulta: comando 1 = consultar por nombre, comando 2 = llenar tabla
        [HttpGet("consultaDpto")]
        public IQueryable Get(int comando, string dato)
        {
            clsOpeDepto oX = new clsOpeDepto(ctx);
            switch (comando)
            {
                case 1: // Consultar un departamento por nombre
                    return oX.dptoXNombre(dato);
                case 2: // Llenar tabla
                    return oX.listarDptosTabla();
                default:
                    return Enumerable.Empty<string>().AsQueryable();
            }
        }

        //Servicio para agregar un registro
        [HttpPost]
        public int Post([FromBody] TblDpto datIn)
        {
            clsOpeDepto oX = new clsOpeDepto(ctx);
            oX.tblDpto = datIn;
            return oX.Agregar();
        }

        //Servicio para modificar un registro
        [HttpPut]
        public int Put([FromBody] TblDpto datIn)
        {
            clsOpeDepto oX = new clsOpeDepto(ctx);
            oX.tblDpto = datIn;
            return oX.Modificar();
        }
    }
}
