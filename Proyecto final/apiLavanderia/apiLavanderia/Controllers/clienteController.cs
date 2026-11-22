using Microsoft.AspNetCore.Mvc;
using apiLavanderia.Models;
using apiLavanderia.clases;

namespace apiLavanderia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class clienteController : ControllerBase
    {
        private readonly LavanderiaContext ctx;

        public clienteController(LavanderiaContext ctx)
        {
            this.ctx = ctx;
        }

        //Combo: clientes (cboCliente de los formularios)
        [HttpGet("listadoClientes")]
        public List<TblCliente> listarClientes()
        {
            clsOpeCliente x = new clsOpeCliente(ctx);
            return x.listaClientes();
        }

        //Direcciones de un cliente (detalle)
        [HttpGet("listadoDirXCliente")]
        public IQueryable listarDireccionesXCliente(int codCliente)
        {
            clsOpeCliente x = new clsOpeCliente(ctx);
            return x.direccionesXCliente(codCliente);
        }

        //Telefonos de un cliente (detalle)
        [HttpGet("listadoTelXCliente")]
        public IQueryable listarTelefonosXCliente(int codCliente)
        {
            clsOpeCliente x = new clsOpeCliente(ctx);
            return x.telefonosXCliente(codCliente);
        }

        //Servicio de consulta: comando 1 = consultar por documento, comando 2 = llenar tabla
        [HttpGet("consultaCliente")]
        public IQueryable Get(int comando, string dato)
        {
            clsOpeCliente oX = new clsOpeCliente(ctx);
            switch (comando)
            {
                case 1: // Consultar un cliente por documento
                    return oX.clienteXDocumento(dato);
                case 2: // Llenar tabla
                    return oX.listarClientesTabla();
                default:
                    return Enumerable.Empty<string>().AsQueryable();
            }
        }

        //Servicio para agregar una direccion a un cliente
        [HttpPost("agregarDireccion")]
        public int AgregarDireccion([FromBody] TblDireccionCliente dir)
        {
            clsOpeCliente oX = new clsOpeCliente(ctx);
            return oX.AgregarDireccion(dir);
        }

        //Servicio para agregar un telefono a un cliente
        [HttpPost("agregarTelefono")]
        public int AgregarTelefono([FromBody] TblTelefonoCliente tel)
        {
            clsOpeCliente oX = new clsOpeCliente(ctx);
            return oX.AgregarTelefono(tel);
        }

        //Servicio para modificar una direccion de un cliente
        [HttpPut("modificarDireccion")]
        public int ModificarDireccion([FromBody] TblDireccionCliente dir)
        {
            clsOpeCliente oX = new clsOpeCliente(ctx);
            return oX.ModificarDireccion(dir);
        }

        //Servicio para modificar un telefono de un cliente
        [HttpPut("modificarTelefono")]
        public int ModificarTelefono([FromBody] TblTelefonoCliente tel)
        {
            clsOpeCliente oX = new clsOpeCliente(ctx);
            return oX.ModificarTelefono(tel);
        }

        //Servicio para agregar un cliente con su detalle
        [HttpPost]
        public int Post([FromBody] clsClienteIn datIn)
        {
            clsOpeCliente oX = new clsOpeCliente(ctx);
            return oX.Agregar(datIn);
        }

        //Servicio para modificar un cliente y su detalle
        [HttpPut]
        public int Put([FromBody] clsClienteIn datIn)
        {
            clsOpeCliente oX = new clsOpeCliente(ctx);
            return oX.Modificar(datIn);
        }
    }
}
