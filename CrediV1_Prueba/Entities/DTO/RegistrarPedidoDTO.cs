namespace CrediV1_Prueba.Entities.DTO
{
    public class RegistrarPedidoDTO
    {
        public int idDetalle {  get; set; }
        public int idPedido {  get; set; }
        public int idProducto { get; set; }
        public int idEmpleado { get; set; }
        public int idProveedor { get; set; }
        public int cantidad {  get; set; }
        public int EmpleadoRecibido { get; set; }
        public int cantidadRecibida { get; set; }

        public float montoUnitario { get; set; }
        public float montoTotalProducto { get; set; }
        public float montoTotalPedido { get; set; }


        public DateTime fechaEncargo { get; set; }
        public DateTime fechaRecibido { get; set; }
        public string? Estado { get; set; }
        public string? EstadoProducto { get; set; }
        public string? observaciones { get; set; }
    }
}
