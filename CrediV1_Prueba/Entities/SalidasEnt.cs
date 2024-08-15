using System.ComponentModel.DataAnnotations;

namespace CrediV1_Prueba.Entities
{
    public class SalidasEnt
    {
        public long idSalida { get; set; }
        public long idCliente { set; get; }
        public string nombreCliente { get; set; }
        public decimal costoVenta { get; set; }
        public int cantidad { get; set; }
        public string metodoPago { get; set; }
        public long idMetodoPago { get; set; }
        public decimal MontoDeVenta { get; set; }
        public decimal ganancia { get; set; }
        public string vendedor { get; set; }
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

        //Creditos
        public long idLinea { get; set; }
        public long idCuenta { get; set; }
        public DateTime fechaAbono { get; set; }
        public decimal montoPagado { get; set; }
        public decimal abono { get; set; }
        public decimal saldo { get; set; }
        public DateTime proximoPago { get; set; }
        public decimal prima { get; set; }
        public int numeroCredito { get; set; }
        public decimal primaCredito { get; set; }
        [Range(1, 12, ErrorMessage = "El plazo en meses debe estar entre 1 y 12.")]
        public int plazo { get; set; }
        public bool cuentaCancelada { get; set; }



    }
}