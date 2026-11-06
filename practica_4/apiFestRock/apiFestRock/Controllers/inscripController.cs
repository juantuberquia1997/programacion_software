using Microsoft.AspNetCore.Mvc;
using apiFestRock.Models;
using apiFestRock.clases;

namespace apiFestRock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class inscripController : ControllerBase
    {
        private readonly BdEventosContext ctx;

        public inscripController(BdEventosContext ctx)
        {
            this.ctx = ctx;
        }

        //Servicio de consulta: comando 1 = consultar por código,
        //comando 2 = llenar tabla, comando 3 = detalle de una inscripción,
        //comando 4 = consultar por evento (dato) y banda (dato2)
        [HttpGet("consultaInscrip")]
        public IQueryable Get(int comando, int dato, int dato2 = 0)
        {
            clsOpeInscrip oX = new clsOpeInscrip(ctx);
            switch (comando)
            {
                case 1: // Consultar una inscripción por su código
                    return oX.inscripXId(dato);
                case 2: // Llenar tabla
                    return oX.listarInscripsTabla();
                case 3: // Detalle de la inscripción (artistas inscritos)
                    return oX.detalleInsc(dato);
                case 4: // Consultar por evento y banda
                    return oX.inscripXEventoBanda(dato, dato2);
                default:
                    return Enumerable.Empty<string>().AsQueryable();
            }
        }

        //Servicio para agregar una inscripción con su detalle de artistas
        [HttpPost]
        public int Post([FromBody] clsInscripIn datIn)
        {
            clsOpeInscrip oX = new clsOpeInscrip(ctx);
            return oX.Agregar(datIn);
        }

        //Servicio para modificar una inscripción (el valor no se recalcula)
        [HttpPut]
        public int Put([FromBody] clsInscripIn datIn)
        {
            clsOpeInscrip oX = new clsOpeInscrip(ctx);
            return oX.Modificar(datIn);
        }
    }
}
