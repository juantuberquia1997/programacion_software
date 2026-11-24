namespace apiLavanderia.clases.auxiliar
{
    //Clases auxiliares (DTOs) para grabar el ingreso completo:
    //encabezado + detalle de prendas a lavar
    public class clsIngresoCompleto
    {
        //Datos de cada prenda del detalle del ingreso. El valor se calcula
        //automaticamente en la API aplicando el descuento por cantidad (RF07)
        public class detalleIngreso
        {
            public int IdTipoPrendaServicio { get; set; }
            public int IdEstadoPrenda { get; set; }
            public int Cantidad { get; set; }
            public string? Observacion { get; set; }
        }

        //Datos del encabezado del ingreso con su listado de prendas
        public class ingresoCompleta
        {
            public int IdCliente { get; set; }
            public int IdDireccion { get; set; }
            public int EmpleadoIdEmpleado { get; set; }
            public DateTime Fecha { get; set; }
            public bool Domicilio { get; set; }
            public bool Activo { get; set; }
            public required List<detalleIngreso> Detalles { get; set; }
        }
    }
}
