namespace CrediV1_Prueba.Entities
{
    public class ReporteEnt
    {
        
        public int anio {  get; set; }
        public int mes {  get; set; }
        public int TotalVentas {  get; set; }
        public int CantidadVentas { get; set; }
        public int CantidadPagos { get; set; }
        public decimal TotalPagado { get; set; }
        public string descripcion { get; set; }
        public string nombreProducto { get; set; }
        public int dia { get; set; }
        public string NombreCuenta { get; set; }
        public string nombreCliente { get; set; }
        public string apellidosCliente { get; set; }
        public string nombreVendedor { get; set; }

        public string metodoPago { get; set; }
        public int cantidadProducto { get; set; }
        public float montoVenta { get; set; }
        public float costoVenta     { get; set; }
        public float ganancia { get; set; }
        public DateTime fecha { get; set; }
        public float comisionVendedor { get; set; }
        public float comisionVenta { get; set; }

        public int numeroFactura { get; set; }
        public float montoPagado { get; set; }


    }
}
