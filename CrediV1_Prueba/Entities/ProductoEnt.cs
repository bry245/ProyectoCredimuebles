namespace CrediV1_Prueba.Entities
{
    public class ProductoEnt
    {
        public long idProducto {  get; set; }
        public string nombre { get; set; }
        public long idCategoria {  get; set; }
        public int cantidadStock { get; set; }
        public int cantidadSalida { get; set; }
        public decimal costo { get; set; }
        public bool estado { get; set; }
        public string costoReferencia { get; set; }
        public long idSalida { get; set; }

        public decimal CostoProveedor { get; set; }
       
        public long idProveedor { get; set; }

    
        public string nombreProveedor { get; set; }


    

        public string categoriaNombre { get; set; }
    }
}
