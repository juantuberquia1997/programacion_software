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

        //Servicio de consulta: comando 1 = consultar por nombre, comando 2 = llenar tabla
        [HttpGet("consultaGenero")]
        public IQueryable Get(int comando, string dato)
        {
            clsOpeGenero oX = new clsOpeGenero(ctx);
            switch (comando)
            {
                case 1: // Consultar un género por nombre
                    return oX.generoXNombre(dato);
                case 2: // Llenar tabla
                    return oX.listarGenerosTabla();
                default:
                    return Enumerable.Empty<string>().AsQueryable();
            }
        }

        //Servicio para agregar un registro
        [HttpPost]
        public int Post([FromBody] TblGenero datIn)
        {
            clsOpeGenero oX = new clsOpeGenero(ctx);
            oX.tblGene = datIn;
            return oX.Agregar();
        }

        //Servicio para modificar un registro
        [HttpPut]
        public int Put([FromBody] TblGenero datIn)
        {
            clsOpeGenero oX = new clsOpeGenero(ctx);
            oX.tblGene = datIn;
            return oX.Modificar();
        }
    }
}
