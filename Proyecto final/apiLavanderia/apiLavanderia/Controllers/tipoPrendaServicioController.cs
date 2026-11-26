using Microsoft.AspNetCore.Mvc;
using apiLavanderia.Models;
using apiLavanderia.clases;

namespace apiLavanderia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tipoPrendaServicioController : ControllerBase
    {
        private readonly LavanderiaContext ctx;

        public tipoPrendaServicioController(LavanderiaContext ctx)
        {
            this.ctx = ctx;
        }

        //Combo de tarifas activas con descripcion (servicio - prenda)
        [HttpGet("comboTipoPrendaServicios")]
        public IQueryable comboTipoPrendaServicios()
        {
            clsOpeTipoPrendaServicio x = new clsOpeTipoPrendaServicio(ctx);
            return x.comboTipoPrendaServicios();
        }

        //Servicio para agregar un registro
        [HttpPost]
        public int Post([FromBody] TblTipoPrendaServicio datIn)
        {
            clsOpeTipoPrendaServicio oX = new clsOpeTipoPrendaServicio(ctx);
            oX.tblTipoPrendaServicio = datIn;
            return oX.Agregar();
        }

        //Servicio para modificar un registro
        [HttpPut]
        public int Put([FromBody] TblTipoPrendaServicio datIn)
        {
            clsOpeTipoPrendaServicio oX = new clsOpeTipoPrendaServicio(ctx);
            oX.tblTipoPrendaServicio = datIn;
            return oX.Modificar();
        }
    }
}
