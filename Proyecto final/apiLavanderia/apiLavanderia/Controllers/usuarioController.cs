using Microsoft.AspNetCore.Mvc;
using apiLavanderia.Models;
using apiLavanderia.clases;

namespace apiLavanderia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usuarioController : ControllerBase
    {
        private readonly LavanderiaContext ctx;

        public usuarioController(LavanderiaContext ctx)
        {
            this.ctx = ctx;
        }

        [HttpGet("listadoUsuariosTodos")]
        public IQueryable listarUsuariosTodos()
        {
            clsOpeUsuario x = new clsOpeUsuario(ctx);
            return x.listarUsuariosTodos();
        }

        //Validar el login del usuario (usuario + clave)
        [HttpGet("validarLogin")]
        public IQueryable validarLogin(string usuario, string clave)
        {
            clsOpeUsuario oX = new clsOpeUsuario(ctx);
            return oX.validarLogin(usuario, clave);
        }

        //Servicio de consulta: comando 1 = consultar por nombre, comando 2 = llenar tabla
        [HttpGet("consultaUsuario")]
        public IQueryable Get(int comando, string dato)
        {
            clsOpeUsuario oX = new clsOpeUsuario(ctx);
            switch (comando)
            {
                case 1: // Consultar un usuario por su nombre de usuario
                    return oX.usuarioXNombre(dato);
                default:
                    return Enumerable.Empty<string>().AsQueryable();
            }
        }

        //Servicio para agregar un registro
        [HttpPost]
        public int Post([FromBody] TblUsuario datIn)
        {
            clsOpeUsuario oX = new clsOpeUsuario(ctx);
            oX.tblUsuario = datIn;
            return oX.Agregar();
        }

        //Servicio para modificar un registro
        [HttpPut]
        public int Put([FromBody] TblUsuario datIn)
        {
            clsOpeUsuario oX = new clsOpeUsuario(ctx);
            oX.tblUsuario = datIn;
            return oX.Modificar();
        }
    }
}
