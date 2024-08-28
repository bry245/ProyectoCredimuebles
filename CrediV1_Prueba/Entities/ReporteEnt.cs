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
    }
}
