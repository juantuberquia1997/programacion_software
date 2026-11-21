using Microsoft.AspNetCore.Mvc;
using apiLavanderia.Models;
using apiLavanderia.clases;

namespace apiLavanderia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tipoPrendaController : ControllerBase
    {
        private readonly LavanderiaContext ctx;

        public tipoPrendaController(LavanderiaContext ctx)
        {
            this.ctx = ctx;
        }

        [HttpGet("listadoTipoPrendas")]
        public List<TblTipoPrenda> listarTipoPrendas()
        {
            clsOpeTipoPrenda x = new clsOpeTipoPrenda(ctx);
            return x.listaTipoPrendas();
        }

        //Servicio de consulta: comando 1 = consultar por nombre, comando 2 = llenar tabla
        [HttpGet("consultaTipoPrenda")]
        public IQueryable Get(int comando, string dato)
        {
            clsOpeTipoPrenda oX = new clsOpeTipoPrenda(ctx);
            switch (comando)
            {
                case 1: // Consultar un tipo de prenda por nombre
                    return oX.tipoPrendaXNombre(dato);
                case 2: // Llenar tabla
                    return oX.listarTipoPrendasTabla();
                default:
                    return Enumerable.Empty<string>().AsQueryable();
            }
        }

        //Servicio para agregar un registro
        [HttpPost]
        public int Post([FromBody] TblTipoPrenda datIn)
        {
            clsOpeTipoPrenda oX = new clsOpeTipoPrenda(ctx);
            oX.tblTipoPrenda = datIn;
            return oX.Agregar();
        }

        //Servicio para modificar un registro
        [HttpPut]
        public int Put([FromBody] TblTipoPrenda datIn)
        {
            clsOpeTipoPrenda oX = new clsOpeTipoPrenda(ctx);
            oX.tblTipoPrenda = datIn;
            return oX.Modificar();
        }
    }
}
