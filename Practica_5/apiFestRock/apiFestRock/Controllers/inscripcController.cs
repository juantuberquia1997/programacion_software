using Microsoft.AspNetCore.Mvc;
using apiFestRock.Models;
using apiFestRock.clases;
using static apiFestRock.clases.auxiliar.clsInscripcionCompleta;

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

        //Grabar la inscripción completa en la BD (maestro-detalle):
        //retorna el nro. de inscripción generado, 0 si falló el grabado del
        //detalle (deshace el encabezado) o -1 en error general (guía 1.7.4)
        [HttpPost("agregarInscrip")]
        public int Post([FromBody] inscripCompleta incripcWeb)
        {
            try
            {
                // 1. Guardar el encabezado
                clsOpeInscrip opeI = new clsOpeInscrip(ctx);
                int idNroInsc = opeI.registrarEncabezado(incripcWeb);

                // 2. Guardar el detalle de artistas
                clsOpeDetInscrip opeDI = new clsOpeDetInscrip(ctx);
                if (opeDI.registrarDetalles(idNroInsc, incripcWeb.Inscritos))
                {
                    return idNroInsc; // Procedimiento con éxito
                }
                else
                {
                    opeI.eliminarEncabezado(idNroInsc); // Deshacer encabezado
                    return 0;
                }
            }
            catch
            {
                return -1; // Error general
            }
        }

        //Modificar los datos del encabezado de la inscripción (guía 1.9.2)
        [HttpPut("modificaEncabinscrip")]
        public string Put([FromBody] TblInscrip objIn)
        {
            clsOpeInscrip oX = new clsOpeInscrip(ctx);
            oX.tblInsc = objIn;
            return oX.Modificar();
        }
    }
}
