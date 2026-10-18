using apiFestRock.Models;
using System.Formats.Cbor;

namespace apiFestRock.clases
{
    public class clsOpeGenero
    {
        private readonly BdEventosContext ctx;

        public clsOpeGenero(BdEventosContext ctx)
        {
            this.ctx = ctx;
        }

        //sentencia sql
        public List<TblGenero> listaGeneros() {
            return ctx.TblGeneros
                .Where(x => x.Activo == true)
                .OrderBy(x => x.Nombre)
                .ToList();
        }
    }
}
