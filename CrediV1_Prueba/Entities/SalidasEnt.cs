namespace CrediV1_Prueba.Entities
{
    public class SalidasEnt
    {
        public long idSalida { get; set; }
        public long idCliente {set;get;}
        public string nombreCliente { get; set; }
        public decimal costoVenta { get; set; }
        public int cantidad { get; set; }
        public string metodoPago { get; set; }
        public long idMetodoPago { get; set; }
        public decimal MontoDeVenta { get; set; }
        public decimal ganancia { get; set; }
        public string vendedor  { get; set; }
        public long idVendedor { get; set; }
        public decimal comisionVendedor { get; set; }
        public decimal comisionVenta { get; set; }
        public DateTime fecha { get; set; }
        public string fecha2 { get; set; }
        public string telefonoCliente { get; set; }
        public string direccionCliente { get; set; }
        public string apellidosCliente { get; set; }
        public string cedulaCliente { get; set; }
        public string correoCliente { get; set; }
        public string numeroFactura { get; set; }
        public List<ProductoEnt> productosCompra { get; set; }



       
    }
}
