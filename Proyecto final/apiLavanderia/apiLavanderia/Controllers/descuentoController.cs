using Microsoft.AspNetCore.Mvc;
using apiLavanderia.Models;
using apiLavanderia.clases;

namespace apiLavanderia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class descuentoController : ControllerBase
    {
        private readonly LavanderiaContext ctx;

        public descuentoController(LavanderiaContext ctx)
        {
            this.ctx = ctx;
        }

        //Descuento por tipo de prenda (para calcular el valor del detalle)
        [HttpGet("consultaDescuentoXPrenda")]
        public IQueryable consultarDescuentoXPrenda(int idPrenda)
        {
            clsOpeDescuento oX = new clsOpeDescuento(ctx);
            return oX.descuentoXPrenda(idPrenda);
        } 

        //Listado de descuentos para llenar la tabla
        [HttpGet("listarDescuentosTabla")]
        public IQueryable listarDescuentosTabla()
        {
            clsOpeDescuento oX = new clsOpeDescuento(ctx);
            return oX.listarDescuentosTabla();
        }

        //Servicio para agregar un registro
        [HttpPost]
        public int Post([FromBody] TblDescuento datIn)
        {
            clsOpeDescuento oX = new clsOpeDescuento(ctx);
            oX.tblDescuento = datIn;
            return oX.Agregar();
        }

        //Servicio para modificar un registro
        [HttpPut]
        public int Put([FromBody] TblDescuento datIn)
        {
            clsOpeDescuento oX = new clsOpeDescuento(ctx);
            oX.tblDescuento = datIn;
            return oX.Modificar();
        }
    }
}
