namespace CrediV1_Prueba.Entities.Otros
{
    public class ProductosBajoStock
    {

        public long IdStockBajo { get; set; }
        public long IdProducto { get; set; }
        public string? Nombre { get; set; }
        public int cantidadStock { get; set; }
        public long? IdProveedor { get; set; }

        public DateTime FechaRegistro { get; set; }

        public int telefono { get; set; }
        public string NombreProveedor { get; set; }
        public string NombreProducto{ get; set; }

        public int cantidadRecomendada { get; set; }
    }
}
