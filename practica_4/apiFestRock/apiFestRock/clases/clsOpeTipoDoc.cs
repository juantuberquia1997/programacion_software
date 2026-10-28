using apiFestRock.Models;


namespace apiFestRock.clases
{
    public class clsOpeTipoDoc
    {
        private readonly BdEventosContext ctx;

        public clsOpeTipoDoc(BdEventosContext ctx)
        {
            this.ctx = ctx;
        }

        public List<TblTipoDoc> listaTipDocs()
        {
            return ctx.TblTipoDocs
                .Where(x => x.Activo == true)
                .OrderBy(x => x.Nombre)
                .ToList();
        }
    }
}
