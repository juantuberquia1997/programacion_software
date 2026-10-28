using apiFestRock.Models;

namespace apiFestRock.clases
{
    public class clsOpeDepto
    {
        private readonly BdEventosContext ctx;

        public clsOpeDepto(BdEventosContext ctx)
        {
            this.ctx = ctx;
        }

        public List<TblDpto> listaDptos()
        {
            return ctx.TblDptos
                .OrderBy(x => x.Nombre)
                .ToList();
        }
    }
}
