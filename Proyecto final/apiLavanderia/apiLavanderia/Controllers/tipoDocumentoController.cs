using Microsoft.AspNetCore.Mvc;
using apiLavanderia.Models;
using apiLavanderia.clases;

namespace apiLavanderia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tipoDocumentoController : ControllerBase
    {
        private readonly LavanderiaContext ctx;

        public tipoDocumentoController(LavanderiaContext ctx)
        {
            this.ctx = ctx;
        }

        [HttpGet("listadoTipoDocumentos")]
        public List<TblTipoDocumento> listarTipoDocumentos()
        {
            clsOpeTipoDocumento x = new clsOpeTipoDocumento(ctx);
            return x.listaTipoDocumentos();
        }

        //Servicio de consulta: comando 1 = consultar por nombre, comando 2 = llenar tabla
        [HttpGet("consultaTipoDocumento")]
        public IQueryable Get(int comando, string dato)
        {
            clsOpeTipoDocumento oX = new clsOpeTipoDocumento(ctx);
            switch (comando)
            {
                case 1: // Consultar un tipo de documento por nombre
                    return oX.tipoDocumentoXNombre(dato);
                case 2: // Llenar tabla
                    return oX.listarTipoDocumentosTabla();
                default:
                    return Enumerable.Empty<string>().AsQueryable();
            }
        }

        //Servicio para agregar un registro
        [HttpPost]
        public int Post([FromBody] TblTipoDocumento datIn)
        {
            clsOpeTipoDocumento oX = new clsOpeTipoDocumento(ctx);
            oX.tblTipoDocumento = datIn;
            return oX.Agregar();
        }

        //Servicio para modificar un registro
        [HttpPut]
        public int Put([FromBody] TblTipoDocumento datIn)
        {
            clsOpeTipoDocumento oX = new clsOpeTipoDocumento(ctx);
            oX.tblTipoDocumento = datIn;
            return oX.Modificar();
        }
    }
}
