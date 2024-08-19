namespace CrediV1_Prueba.Entities
{
    public class Gasto
    {
        public int Id { get; set; } // Identificador único del gasto
        public string Descripcion { get; set; } // Descripción del gasto
        public decimal Monto { get; set; } // Monto del gasto
        public DateTime Fecha { get; set; } // Fecha en que se realizó el gasto
    }
}