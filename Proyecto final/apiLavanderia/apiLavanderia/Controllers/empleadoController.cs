using Microsoft.AspNetCore.Mvc;
using apiLavanderia.Models;
using apiLavanderia.clases;

namespace apiLavanderia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class empleadoController : ControllerBase
    {
        private readonly LavanderiaContext ctx;

        public empleadoController(LavanderiaContext ctx)
        {
            this.ctx = ctx;
        }

        //Combo: empleados activos (cboEmpleado de los formularios)
        [HttpGet("listadoEmpleados")]
        public List<TblEmpleado> listarEmpleados()
        {
            clsOpeEmpleado x = new clsOpeEmpleado(ctx);
            return x.listaEmpleados();
        }

        //Profesiones de un empleado (detalle)
        [HttpGet("listadoProfXEmpleado")]
        public IQueryable listarProfesionesXEmpleado(int codEmpleado)
        {
            clsOpeEmpleado x = new clsOpeEmpleado(ctx);
            return x.profesionesXEmpleado(codEmpleado);
        }

        //Direcciones de un empleado (detalle)
        [HttpGet("listadoDirXEmpleado")]
        public IQueryable listarDireccionesXEmpleado(int codEmpleado)
        {
            clsOpeEmpleado x = new clsOpeEmpleado(ctx);
            return x.direccionesXEmpleado(codEmpleado);
        }

        //Telefonos de un empleado (detalle)
        [HttpGet("listadoTelXEmpleado")]
        public IQueryable listarTelefonosXEmpleado(int codEmpleado)
        {
            clsOpeEmpleado x = new clsOpeEmpleado(ctx);
            return x.telefonosXEmpleado(codEmpleado);
        }

        //Servicio de consulta: comando 1 = consultar por documento, comando 2 = llenar tabla
        [HttpGet("consultaEmpleado")]
        public IQueryable Get(int comando, string dato)
        {
            clsOpeEmpleado oX = new clsOpeEmpleado(ctx);
            switch (comando)
            {
                case 1: // Consultar un empleado por documento
                    return oX.empleadoXDocumento(dato);
                case 2: // Llenar tabla
                    return oX.listarEmpleadosTabla();
                default:
                    return Enumerable.Empty<string>().AsQueryable();
            }
        }

        //Servicio para agregar una direccion a un empleado
        [HttpPost("agregarDireccion")]
        public int AgregarDireccion([FromBody] TblDireccionEmpleado dir)
        {
            clsOpeEmpleado oX = new clsOpeEmpleado(ctx);
            return oX.AgregarDireccion(dir);
        }

        //Servicio para agregar un telefono a un empleado
        [HttpPost("agregarTelefono")]
        public int AgregarTelefono([FromBody] TblTelefonoEmpleado tel)
        {
            clsOpeEmpleado oX = new clsOpeEmpleado(ctx);
            return oX.AgregarTelefono(tel);
        }

        //Servicio para modificar una direccion de un empleado
        [HttpPut("modificarDireccion")]
        public int ModificarDireccion([FromBody] TblDireccionEmpleado dir)
        {
            clsOpeEmpleado oX = new clsOpeEmpleado(ctx);
            return oX.ModificarDireccion(dir);
        }

        //Servicio para modificar un telefono de un empleado
        [HttpPut("modificarTelefono")]
        public int ModificarTelefono([FromBody] TblTelefonoEmpleado tel)
        {
            clsOpeEmpleado oX = new clsOpeEmpleado(ctx);
            return oX.ModificarTelefono(tel);
        }

        //Servicio para agregar un empleado con su detalle
        [HttpPost]
        public int Post([FromBody] clsEmpleadoIn datIn)
        {
            clsOpeEmpleado oX = new clsOpeEmpleado(ctx);
            return oX.Agregar(datIn);
        }

        //Servicio para modificar un empleado y su detalle
        [HttpPut]
        public int Put([FromBody] clsEmpleadoIn datIn)
        {
            clsOpeEmpleado oX = new clsOpeEmpleado(ctx);
            return oX.Modificar(datIn);
        }
    }
}
