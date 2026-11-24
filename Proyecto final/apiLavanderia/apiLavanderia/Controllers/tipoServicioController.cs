using Microsoft.AspNetCore.Mvc;
using apiLavanderia.Models;
using apiLavanderia.clases;

namespace apiLavanderia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tipoServicioController : ControllerBase
    {
        private readonly LavanderiaContext ctx;

        public tipoServicioController(LavanderiaContext ctx)
        {
            this.ctx = ctx;
        }

        //Servicio de consulta: comando 1 = consultar por nombre, comando 2 = llenar tabla
        [HttpGet("consultaTipoServicio")]
        public IQueryable Get(int comando, string dato)
        {
            clsOpeTipoServicio oX = new clsOpeTipoServicio(ctx);
            switch (comando)
            {
                case 1: // Consultar un tipo de servicio por nombre
                    return oX.tipoServicioXNombre(dato);
                case 2: // Llenar tabla
                    return oX.listarTipoServiciosTabla();
                default:
                    return Enumerable.Empty<string>().AsQueryable();
            }
        }

        //Servicio para agregar un registro
        [HttpPost]
        public int Post([FromBody] TblTipoServicio datIn)
        {
            clsOpeTipoServicio oX = new clsOpeTipoServicio(ctx);
            oX.tblTipoServicio = datIn;
            return oX.Agregar();
        }

        //Servicio para modificar un registro
        [HttpPut]
        public int Put([FromBody] TblTipoServicio datIn)
        {
            clsOpeTipoServicio oX = new clsOpeTipoServicio(ctx);
            oX.tblTipoServicio = datIn;
            return oX.Modificar();
        }
    }
}
