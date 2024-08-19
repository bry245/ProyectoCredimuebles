namespace CrediV1_Prueba.Entities
{
    public class CajaEnt
    {
        public DateTime fechaID  { get; set; }
        public int totalVentas { get; set; }
        public decimal montoVentas { get; set; }
        public decimal comisiones { get; set; }
        public long idGasto { get; set; }
        public decimal dineroDeVentas { get; set; }
        public decimal ganancia { get; set; }

        public decimal montoGastos { get; set; }
        public DateTime fechaGasto { get; set; }
        public string descripcion { get; set; }
    }
}
