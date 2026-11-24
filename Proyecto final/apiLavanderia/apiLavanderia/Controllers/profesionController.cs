using Microsoft.AspNetCore.Mvc;
using apiLavanderia.Models;
using apiLavanderia.clases;

namespace apiLavanderia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class profesionController : ControllerBase
    {
        private readonly LavanderiaContext ctx;

        public profesionController(LavanderiaContext ctx)
        {
            this.ctx = ctx;
        }

        [HttpGet("listadoProfesiones")]
        public List<TblProfesion> listarProfesiones()
        {
            clsOpeProfesion x = new clsOpeProfesion(ctx);
            return x.listaProfesiones();
        }

        //Servicio de consulta: comando 1 = consultar por nombre, comando 2 = llenar tabla
        [HttpGet("consultaProfesion")]
        public IQueryable Get(int comando, string dato)
        {
            clsOpeProfesion oX = new clsOpeProfesion(ctx);
            switch (comando)
            {
                case 1: // Consultar una profesion por nombre
                    return oX.profesionXNombre(dato);
                case 2: // Llenar tabla
                    return oX.listarProfesionesTabla();
                default:
                    return Enumerable.Empty<string>().AsQueryable();
            }
        }

        //Servicio para agregar un registro
        [HttpPost]
        public int Post([FromBody] TblProfesion datIn)
        {
            clsOpeProfesion oX = new clsOpeProfesion(ctx);
            oX.tblProfesion = datIn;
            return oX.Agregar();
        }

        //Servicio para modificar un registro
        [HttpPut]
        public int Put([FromBody] TblProfesion datIn)
        {
            clsOpeProfesion oX = new clsOpeProfesion(ctx);
            oX.tblProfesion = datIn;
            return oX.Modificar();
        }
    }
}
