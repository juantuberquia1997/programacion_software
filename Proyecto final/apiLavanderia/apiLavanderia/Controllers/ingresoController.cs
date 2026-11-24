using Microsoft.AspNetCore.Mvc;
using apiLavanderia.Models;
using apiLavanderia.clases;
using static apiLavanderia.clases.auxiliar.clsIngresoCompleto;

namespace apiLavanderia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ingresoController : ControllerBase
    {
        private readonly LavanderiaContext ctx;

        public ingresoController(LavanderiaContext context)
        {
            ctx = context;
        }

        //Servicio de consulta del encabezado de un ingreso por su codigo
        [HttpGet("buscarIngreso")]
        public IQueryable Get(int codIngreso)
        {
            clsOpeIngreso oX = new clsOpeIngreso(ctx);
            return oX.consultarIngreso(codIngreso);
        }

        //Detalle de un ingreso (prendas a lavar con su estado)
        [HttpGet("consultarDetalleIngreso")]
        public IQueryable GetDetalle(int codIngreso)
        {
            clsOpeIngreso oX = new clsOpeIngreso(ctx);
            return oX.detalleIngreso(codIngreso);
        }

        //Servicio de consulta: comando 2 = llenar tabla
        [HttpGet("consultaIngreso")]
        public IQueryable GetTabla(int comando, string dato)
        {
            clsOpeIngreso oX = new clsOpeIngreso(ctx);
            switch (comando)
            {
                case 2: // Llenar tabla
                    return oX.listarIngresosTabla();
                default:
                    return Enumerable.Empty<string>().AsQueryable();
            }
        }

        //Grabar el ingreso completo en la BD (maestro-detalle):
        //retorna el codigo de ingreso generado, 0 si fallo el grabado del
        //detalle (deshace el encabezado) o -1 en error general
        [HttpPost("agregarIngreso")]
        public int Post([FromBody] ingresoCompleta ingresoWeb)
        {
            try
            {
                // 1. Guardar el encabezado
                clsOpeIngreso opeI = new clsOpeIngreso(ctx);
                int idIngreso = opeI.registrarEncabezado(ingresoWeb);

                // 2. Guardar el detalle de prendas
                if (opeI.registrarDetalles(idIngreso, ingresoWeb.IdCliente, ingresoWeb.Detalles))
                {
                    return idIngreso; // Procedimiento con exito
                }
                else
                {
                    opeI.eliminarEncabezado(idIngreso); // Deshacer encabezado
                    return 0;
                }
            }
            catch
            {
                return -1; // Error general
            }
        }

        //Modificar los datos del encabezado del ingreso
        [HttpPut("modificaEncabingreso")]
        public string Put([FromBody] TblIngreso objIn)
        {
            clsOpeIngreso oX = new clsOpeIngreso(ctx);
            oX.tblIngreso = objIn;
            return oX.Modificar();
        }
    }
}
