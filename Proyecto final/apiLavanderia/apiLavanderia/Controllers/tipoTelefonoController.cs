using Microsoft.AspNetCore.Mvc;
using apiLavanderia.Models;
using apiLavanderia.clases;

namespace apiLavanderia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tipoTelefonoController : ControllerBase
    {
        private readonly LavanderiaContext ctx;

        public tipoTelefonoController(LavanderiaContext ctx)
        {
            this.ctx = ctx;
        }

        [HttpGet("listadoTipoTelefonos")]
        public List<TblTipoTelefono> listarTipoTelefonos()
        {
            clsOpeTipoTelefono x = new clsOpeTipoTelefono(ctx);
            return x.listaTipoTelefonos();
        }

        //Servicio de consulta: comando 1 = consultar por nombre, comando 2 = llenar tabla
        [HttpGet("consultaTipoTelefono")]
        public IQueryable Get(int comando, string dato)
        {
            clsOpeTipoTelefono oX = new clsOpeTipoTelefono(ctx);
            switch (comando)
            {
                case 1: // Consultar un tipo de telefono por nombre
                    return oX.tipoTelefonoXNombre(dato);
                case 2: // Llenar tabla
                    return oX.listarTipoTelefonosTabla();
                default:
                    return Enumerable.Empty<string>().AsQueryable();
            }
        }

        //Servicio para agregar un registro
        [HttpPost]
        public int Post([FromBody] TblTipoTelefono datIn)
        {
            clsOpeTipoTelefono oX = new clsOpeTipoTelefono(ctx);
            oX.tblTipoTelefono = datIn;
            return oX.Agregar();
        }

        //Servicio para modificar un registro
        [HttpPut]
        public int Put([FromBody] TblTipoTelefono datIn)
        {
            clsOpeTipoTelefono oX = new clsOpeTipoTelefono(ctx);
            oX.tblTipoTelefono = datIn;
            return oX.Modificar();
        }
    }
}
