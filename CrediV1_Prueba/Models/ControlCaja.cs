using System.ComponentModel.DataAnnotations;
using CrediV1_Prueba.Entities.Otros;
using CrediV1_Prueba.Interfaces;
using System.Data.SqlClient;
using System.Data;
using CrediV1_Prueba.Entities.DTO;

namespace CrediV1_Prueba.Models
{
    public class ControlCaja
    {
        [Key]
        public DateTime fechaID { get; set; }
        public int? totalventas { get; set; }
        public decimal? montoVentas { get; set; }
        public decimal? ganancia { get; set; }
        public decimal? comisiones { get; set; }
        public long? idGasto { get; set; }
        public decimal? totalCaja { get; set; }
    }
}
