namespace CrediV1_Prueba.Entities
{
    public class PedidoEnt
    {

        public int idPedido {  get; set; }
        public int idProducto { get; set; }
        public int idEmpleado { get; set; }
        public int idProveedor { get; set; }
        public int idDetalle { get; set; }
        public int EmpleadoRecibido { get; set; }
        public float montoUnitario { get; set; }
        public float montoTotalProducto { get; set; }
        public float CostoProveedor { get; set; }
        public float montoTotalPedido { get; set; }
        public DateTime fechaEncargo { get; set; }
        public DateTime? fechaRecibido { get; set; }
        public int cantidad { get; set; }
        public string? estado {  get; set; }

        public string? correoProveedor { get; set; }
        public string? nombreProducto { get; set; }
        public string? nombreProveedor { get; set; }
        public string? nombreEmpleado { get; set; }

    }
}
