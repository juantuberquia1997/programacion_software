using Microsoft.AspNetCore.Mvc;
using apiFestRock.Models;
using apiFestRock.clases;


namespace apiFestRock.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ciudadController : ControllerBase
    {

        private readonly BdEventosContext ctx;

        public ciudadController(BdEventosContext ctx)
        {
            this.ctx = ctx;
        }

        [HttpGet("listadoCiudsXDpto")]
        public List<TblCiudad> listarCiudadesXDpto(int codDpto)
        {
            clsOpeCiudad x = new clsOpeCiudad(ctx);
            return x.listaCiudadesxDpto(codDpto);
        }

        //Servicio de consulta: comando 1 = consultar por nombre, comando 2 = llenar tabla
        [HttpGet("consultaCiudad")]
        public IQueryable Get(int comando, string dato)
        {
            clsOpeCiudad oX = new clsOpeCiudad(ctx);
            switch (comando)
            {
                case 1: // Consultar una ciudad por nombre
                    return oX.ciudadXNombre(dato);
                case 2: // Llenar tabla
                    return oX.listarCiudadsTabla();
                default:
                    return Enumerable.Empty<string>().AsQueryable();
            }
        }

        //Servicio para agregar un registro
        [HttpPost]
        public int Post([FromBody] TblCiudad datIn)
        {
            clsOpeCiudad oX = new clsOpeCiudad(ctx);
            oX.tblCiud = datIn;
            return oX.Agregar();
        }

        //Servicio para modificar un registro
        [HttpPut]
        public int Put([FromBody] TblCiudad datIn)
        {
            clsOpeCiudad oX = new clsOpeCiudad(ctx);
            oX.tblCiud = datIn;
            return oX.Modificar();
        }
    }
}
