using Microsoft.AspNetCore.Mvc;
using apiFestRock.Models;
using apiFestRock.clases;

namespace apiFestRock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class bandaController : ControllerBase
    {
        private readonly BdEventosContext ctx;

        public bandaController(BdEventosContext ctx)
        {
            this.ctx = ctx;
        }

        //Combo: bandas activas (cboBanda del form Inscripción)
        [HttpGet("listadoBandas")]
        public List<TblBandum> listarBandas()
        {
            clsOpeBanda x = new clsOpeBanda(ctx);
            return x.listaBandas();
        }

        //Artistas de una banda con su instrumento (detalle del form Banda,
        //cboArtBand del form Inscripción y consulta Artista por banda)
        [HttpGet("listadoArtXBand")]
        public IQueryable listarArtistasXBanda(int codBanda)
        {
            clsOpeBanda x = new clsOpeBanda(ctx);
            return x.artistasXBanda(codBanda);
        }

        //Servicio de consulta: comando 1 = consultar por nombre, comando 2 = llenar tabla
        [HttpGet("consultaBanda")]
        public IQueryable Get(int comando, string dato)
        {
            clsOpeBanda oX = new clsOpeBanda(ctx);
            switch (comando)
            {
                case 1: // Consultar una banda por nombre
                    return oX.bandaXNombre(dato);
                case 2: // Llenar tabla
                    return oX.listarBandasTabla();
                default:
                    return Enumerable.Empty<string>().AsQueryable();
            }
        }

        //Servicio para agregar una banda con su detalle de artistas
        [HttpPost]
        public int Post([FromBody] clsBandaIn datIn)
        {
            clsOpeBanda oX = new clsOpeBanda(ctx);
            return oX.Agregar(datIn);
        }

        //Servicio para modificar una banda y su detalle de artistas
        [HttpPut]
        public int Put([FromBody] clsBandaIn datIn)
        {
            clsOpeBanda oX = new clsOpeBanda(ctx);
            return oX.Modificar(datIn);
        }
    }
}
