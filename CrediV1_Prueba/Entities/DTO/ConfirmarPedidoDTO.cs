namespace CrediV1_Prueba.Entities.DTO
{
    public class ConfirmarPedidoDTO
    {

        public int idPedido { get; set; }
        public int idDetalle { get; set; }
        public string? observaciones { get; set; }
        public string ?emailProveedor { get; set; }
        public string estadoProducto { get; set; }

    }
}
