using Microsoft.AspNetCore.Mvc;
using apiLavanderia.Models;
using apiLavanderia.clases;

namespace apiLavanderia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class departamentoController : ControllerBase
    {
        private readonly LavanderiaContext ctx;

        public departamentoController(LavanderiaContext ctx)
        {
            this.ctx = ctx;
        }

        [HttpGet("listadoDepartamentos")]
        public List<TblDepartamento> listarDepartamentos()
        {
            clsOpeDepartamento x = new clsOpeDepartamento(ctx);
            return x.listaDepartamentos();
        }

        //Servicio de consulta: comando 1 = consultar por nombre, comando 2 = llenar tabla
        [HttpGet("consultaDepartamento")]
        public IQueryable Get(int comando, string dato)
        {
            clsOpeDepartamento oX = new clsOpeDepartamento(ctx);
            switch (comando)
            {
                case 1: // Consultar un departamento por nombre
                    return oX.departamentoXNombre(dato);
                case 2: // Llenar tabla
                    return oX.listarDepartamentosTabla();
                default:
                    return Enumerable.Empty<string>().AsQueryable();
            }
        }

        //Servicio para agregar un registro
        [HttpPost]
        public int Post([FromBody] TblDepartamento datIn)
        {
            clsOpeDepartamento oX = new clsOpeDepartamento(ctx);
            oX.tblDepartamento = datIn;
            return oX.Agregar();
        }

        //Servicio para modificar un registro
        [HttpPut]
        public int Put([FromBody] TblDepartamento datIn)
        {
            clsOpeDepartamento oX = new clsOpeDepartamento(ctx);
            oX.tblDepartamento = datIn;
            return oX.Modificar();
        }
    }
}
