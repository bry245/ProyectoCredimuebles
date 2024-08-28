using CrediV1_Prueba.Entities;

namespace CrediV1_Prueba.Interfaces
{
    public interface IReporteService
    {
        public Task<IEnumerable<ReporteEnt>> VentasMensuales();
        public Task<IEnumerable<ReporteEnt>> VentasPorMetodoPago();
        public Task<IEnumerable<ReporteEnt>> VentasPorMetodoPagoCantidad();

        public Task<ReporteEnt> VentasDia();
        public Task<ReporteEnt> ObtenerAbonosSemanales();


        public Task<IEnumerable<ReporteEnt>> ObtenerAbonosMensuales();

    }

