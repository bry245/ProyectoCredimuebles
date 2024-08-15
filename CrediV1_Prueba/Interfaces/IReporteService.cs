using CrediV1_Prueba.Entities;

namespace CrediV1_Prueba.Interfaces
{
    public interface IReporteService
    {
        public Task<IEnumerable<ReporteEnt>> VentasMensuales();
        public Task<IEnumerable<SalidasEnt>> VentasPorMetodoPago();
        public Task<IEnumerable<SalidasEnt>> VentasPorMetodoPagoCantidad();

    }
}
