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

        [HttpGet("listadoDescuentos")]
        public List<TblDescuento> listarDescuentos()
        {
            clsOpeDescuento x = new clsOpeDescuento(ctx);
            return x.listaDescuentos();
        }

        //Descuento por tipo de prenda (para calcular el valor del detalle)
        [HttpGet("consultaDescuentoXPrenda")]
        public IQueryable consultarDescuentoXPrenda(int idPrenda)
        {
            clsOpeDescuento oX = new clsOpeDescuento(ctx);
            return oX.descuentoXPrenda(idPrenda);
        }

        //Servicio de consulta: comando 2 = llenar tabla
        [HttpGet("consultaDescuento")]
        public IQueryable Get(int comando, string dato)
        {
            clsOpeDescuento oX = new clsOpeDescuento(ctx);
            switch (comando)
            {
                case 2: // Llenar tabla
                    return oX.listarDescuentosTabla();
                default:
                    return Enumerable.Empty<string>().AsQueryable();
            }
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
