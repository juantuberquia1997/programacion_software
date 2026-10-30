using Microsoft.AspNetCore.Mvc;
using apiFestRock.Models;
using apiFestRock.clases;

namespace apiFestRock.Controllers
{

        [Route("api/[controller]")]
        [ApiController]
    public class artistaController : Controller
    {
        private readonly BdEventosContext ctx;

        public artistaController(BdEventosContext ctx)
        {
            this.ctx = ctx;
        }

        [HttpGet("consultarArtista")]
        public IQueryable Get(int comando, int dato)
        {
            clsOpeArtista oX = new clsOpeArtista(ctx);
            switch (comando)
            {
                case 1: // Consultar un artista por nro. de documento
                    return oX.artistaXNroDoc(dato);
                case 2: // Llenar tabla
                    return oX.listarArtistas();
                default:
                    return Enumerable.Empty<string>().AsQueryable();
            }
        }

        //Servicio para agregar un registro
        [HttpPost]
        public int Post([FromBody] TblArtistum datIn)
        {
            //Crear un objeto desde la clase operativa
            clsOpeArtista oX = new clsOpeArtista(ctx);
            oX.tblArtis = datIn; //Pasar los parámetros del artista
            return oX.Agregar();
        }

        //Servicio para modificar un registro
        [HttpPut]
        public int Put([FromBody] TblArtistum datIn)
        {
            //Crear un objeto desde la clase operativa
            clsOpeArtista oX = new clsOpeArtista(ctx);
            oX.tblArtis = datIn; //Pasar los parámetros del artista
            return oX.Modificar();
        }

        //terminar metodo cancelar y este file
    }
}
