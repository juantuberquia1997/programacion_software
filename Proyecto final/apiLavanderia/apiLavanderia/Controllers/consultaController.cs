using Microsoft.AspNetCore.Mvc;
using apiLavanderia.Models;
using apiLavanderia.clases;

namespace apiLavanderia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class consultaController : ControllerBase
    {
        private readonly LavanderiaContext ctx;

        public consultaController(LavanderiaContext ctx)
        {
            this.ctx = ctx;
        }

        //Ingresos de un cliente
        [HttpGet("ingresosXCliente")]
        public IQueryable GetIngresosXCliente(int idCliente)
        {
            clsOpeConsulta x = new clsOpeConsulta(ctx);
            return x.ingresosXCliente(idCliente);
        }

        //Ingresos realizados por un empleado
        [HttpGet("ingresosXEmpleado")]
        public IQueryable GetIngresosXEmpleado(int idEmpleado)
        {
            clsOpeConsulta x = new clsOpeConsulta(ctx);
            return x.ingresosXEmpleado(idEmpleado);
        }

        //Prendas de un estado (recibida, en lavado, entregada, etc.)
        [HttpGet("prendasXEstado")]
        public IQueryable GetPrendasXEstado(int idEstado)
        {
            clsOpeConsulta x = new clsOpeConsulta(ctx);
            return x.prendasXEstado(idEstado);
        }
    }
}
