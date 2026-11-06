using apiFestRock.Models;
using apiFestRock.clases;
using Microsoft.AspNetCore.Mvc;

namespace apiFestRock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class eventoController : ControllerBase
    {

        private readonly BdEventosContext ctx;
        public eventoController(BdEventosContext ctx)
        {
            this.ctx = ctx;
        }

        //Combo: eventos activos (cboEvento del form Inscripción)
        [HttpGet("listadoEventos")]
        public List<TblEvento> listarEventos()
        {
            clsOpeEvento oX = new clsOpeEvento(ctx);
            return oX.listaEventos();
        }

        //Valor de la inscripción por artista de un evento (lblVrIndInsc)
        [HttpGet("consultaVrInscrip")]
        public IQueryable consultarVrInscrip(int idEvent)
        {
            clsOpeEvento oX = new clsOpeEvento(ctx);
            return oX.consultarVrInscrip(idEvent);
        }

        //Servicio de consulta: comando 1 = consultar por nombre, comando 2 = llenar tabla
        [HttpGet("consultaEvento")]
        public IQueryable Get(int comando, string dato)
        {
            clsOpeEvento oX = new clsOpeEvento(ctx);
            switch (comando)
            {
                case 1: // Consultar un evento por nombre
                    return oX.eventoXNombre(dato);
                case 2: // Llenar tabla
                    return oX.listarEventosTabla();
                default:
                    return Enumerable.Empty<string>().AsQueryable();
            }
        }

        //Servicio para agregar un registro
        [HttpPost]
        public int Post([FromBody] TblEvento datIn)
        {
            clsOpeEvento oX = new clsOpeEvento(ctx);
            oX.tblEvent = datIn;
            return oX.Agregar();
        }

        //Servicio para modificar un registro
        [HttpPut]
        public int Put([FromBody] TblEvento datIn)
        {
            clsOpeEvento oX = new clsOpeEvento(ctx);
            oX.tblEvent = datIn;
            return oX.Modificar();
        }
    }
}
