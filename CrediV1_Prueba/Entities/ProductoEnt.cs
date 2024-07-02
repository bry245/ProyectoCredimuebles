namespace CrediV1_Prueba.Entities
{
    public class ProductoEnt
    {
        public long idProducto { get; set; }

        public long idProveedor { get; set; }

        public string nombre { get; set; }

        public long idCategoria { get; set; }

        public int cantidadStock { get; set; }
        
        public int costo { get; set; }

        public bool estado { get; set; }

        public string CategoriaNombre { get; set; }
    }
}
