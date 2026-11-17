namespace apiFestRock.clases.auxiliar
{
    //Clases auxiliares (DTOs) para grabar la inscripción completa:
    //encabezado + detalle de artistas inscritos (guía 1.7.1)
    public class clsInscripcionCompleta
    {
        //Datos de cada artista del detalle de la inscripción (guía 1.7.1.ii)
        public class detalleInsc
        {
            public int IdArtBand { get; set; }
            public int Edad { get; set; }
        }

        //Datos del encabezado de la inscripción con su listado de
        //artistas inscritos (guía 1.7.1.iii)
        public class inscripCompleta
        {
            public int IdEvento { get; set; }
            public required string Fecha { get; set; }
            public int IdBanda { get; set; }
            public required string Represent { get; set; }
            public required string TelRepresent { get; set; }
            public decimal ValorInscrip { get; set; }
            public required List<detalleInsc> Inscritos { get; set; }
        }
    }
}
