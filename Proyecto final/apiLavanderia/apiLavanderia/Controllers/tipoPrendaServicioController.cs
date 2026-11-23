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

        [HttpGet("listadoTipoPrendaServicios")]
        public List<TblTipoPrendaServicio> listarTipoPrendaServicios()
        {
            clsOpeTipoPrendaServicio x = new clsOpeTipoPrendaServicio(ctx);
            return x.listaTipoPrendaServicios();
        }

        //Combo de tarifas activas con descripcion (servicio - prenda)
        [HttpGet("comboTipoPrendaServicios")]
        public IQueryable comboTipoPrendaServicios()
        {
            clsOpeTipoPrendaServicio x = new clsOpeTipoPrendaServicio(ctx);
            return x.comboTipoPrendaServicios();
        }

        //Valor unitario de un servicio para una prenda (para el detalle del ingreso)
        [HttpGet("consultaVrUnitario")]
        public IQueryable consultarValorUnitario(int idServicio, int idPrenda)
        {
            clsOpeTipoPrendaServicio oX = new clsOpeTipoPrendaServicio(ctx);
            return oX.valorUnitarioXPrendaServicio(idServicio, idPrenda);
        }

        //Servicio de consulta: comando 2 = llenar tabla
        [HttpGet("consultaTipoPrendaServicio")]
        public IQueryable Get(int comando, string dato)
        {
            clsOpeTipoPrendaServicio oX = new clsOpeTipoPrendaServicio(ctx);
            switch (comando)
            {
                case 2: // Llenar tabla
                    return oX.listarTipoPrendaServiciosTabla();
                default:
                    return Enumerable.Empty<string>().AsQueryable();
            }
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
