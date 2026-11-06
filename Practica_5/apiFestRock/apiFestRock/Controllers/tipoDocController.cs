using Microsoft.AspNetCore.Mvc;
using apiFestRock.Models;
using apiFestRock.clases;


namespace apiFestRock.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class tipoDocController : ControllerBase
    {

        private readonly BdEventosContext ctx;

        public tipoDocController(BdEventosContext ctx)
        {
            this.ctx = ctx;
        }

        [HttpGet("listadoTipDocs")]
        public List<TblTipoDoc> listarTiposDoc()
        {
            clsOpeTipoDoc x = new clsOpeTipoDoc(ctx);
            return x.listaTipDocs();
        }

        //Servicio de consulta: comando 1 = consultar por nombre, comando 2 = llenar tabla
        [HttpGet("consultaTipDoc")]
        public IQueryable Get(int comando, string dato)
        {
            clsOpeTipoDoc oX = new clsOpeTipoDoc(ctx);
            switch (comando)
            {
                case 1: // Consultar un tipo de documento por nombre
                    return oX.tipDocXNombre(dato);
                case 2: // Llenar tabla
                    return oX.listarTipDocsTabla();
                default:
                    return Enumerable.Empty<string>().AsQueryable();
            }
        }

        //Servicio para agregar un registro
        [HttpPost]
        public int Post([FromBody] TblTipoDoc datIn)
        {
            clsOpeTipoDoc oX = new clsOpeTipoDoc(ctx);
            oX.tblTipDoc = datIn;
            return oX.Agregar();
        }

        //Servicio para modificar un registro
        [HttpPut]
        public int Put([FromBody] TblTipoDoc datIn)
        {
            clsOpeTipoDoc oX = new clsOpeTipoDoc(ctx);
            oX.tblTipDoc = datIn;
            return oX.Modificar();
        }
    }
}
