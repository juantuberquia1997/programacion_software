
using apiFestRock.Models;

namespace apiFestRock.clases
{
    public class clsOpeCiudad
    {
        private readonly BdEventosContext ctx;

        public clsOpeCiudad(BdEventosContext ctx)
        {
            this.ctx = ctx;
        }

        public List<TblCiudad> listaCiudadesxDpto(int idDpto)
        {
            return ctx.TblCiudads
                .Where(x => x.IdDpto == idDpto)
                .OrderBy(x => x.Nombre)
                .ToList();
        }
    }
}
