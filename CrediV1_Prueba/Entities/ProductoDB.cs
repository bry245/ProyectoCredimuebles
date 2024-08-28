namespace CrediV1_Prueba.Entities
{
	public class ProductoDB
	{
		public long idProducto { get; set; }
		public string? nombre { get; set; }
		public long idCategoria { get; set; }
		public int cantidadStock { get; set; }
		public int cantidadSalida { get; set; }
		public decimal costo { get; set; }
		public bool estado { get; set; }
		public decimal CostoProveedor { get; set; }

		public string? nombreProveedor { get; set; }
		public int telefonoProveedor { get; set; }

		public string? correoProveedor { get; set; }
		public string? direcionProveedor { get; set; }
		public bool estadoProveedor { get; set; }






	}
}
	  