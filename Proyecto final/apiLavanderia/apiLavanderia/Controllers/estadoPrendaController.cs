using Microsoft.AspNetCore.Mvc;
using apiLavanderia.Models;
using apiLavanderia.clases;

namespace apiLavanderia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class estadoPrendaController : ControllerBase
    {
        private readonly LavanderiaContext ctx;

        public estadoPrendaController(LavanderiaContext ctx)
        {
            this.ctx = ctx;
        }

        [HttpGet("listadoEstadoPrendas")]
        public List<TblEstadoPrenda> listarEstadoPrendas()
        {
            clsOpeEstadoPrenda x = new clsOpeEstadoPrenda(ctx);
            return x.listaEstadoPrendas();
        }

        //Servicio de consulta: comando 1 = consultar por nombre, comando 2 = llenar tabla
        [HttpGet("consultaEstadoPrenda")]
        public IQueryable Get(int comando, string dato)
        {
            clsOpeEstadoPrenda oX = new clsOpeEstadoPrenda(ctx);
            switch (comando)
            {
                case 1: // Consultar un estado de prenda por nombre
                    return oX.estadoPrendaXNombre(dato);
                case 2: // Llenar tabla
                    return oX.listarEstadoPrendasTabla();
                default:
                    return Enumerable.Empty<string>().AsQueryable();
            }
        }

        //Servicio para agregar un registro
        [HttpPost]
        public int Post([FromBody] TblEstadoPrenda datIn)
        {
            clsOpeEstadoPrenda oX = new clsOpeEstadoPrenda(ctx);
            oX.tblEstadoPrenda = datIn;
            return oX.Agregar();
        }

        //Servicio para modificar un registro
        [HttpPut]
        public int Put([FromBody] TblEstadoPrenda datIn)
        {
            clsOpeEstadoPrenda oX = new clsOpeEstadoPrenda(ctx);
            oX.tblEstadoPrenda = datIn;
            return oX.Modificar();
        }
    }
}
