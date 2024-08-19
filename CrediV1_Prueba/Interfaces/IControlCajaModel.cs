using CrediV1_Prueba.Models;

namespace CrediV1_Prueba.Interfaces
{
    public interface IControlCajaModel
    {
        Task<List<ControlCaja>> ObtenerControlCaja();
        Task<ControlCaja> BuscarControlCajaPorFecha(DateTime fecha);
        Task<bool> ActualizarControlCaja(ControlCaja controlCaja);
        Task<bool> CrearControlCaja(ControlCaja controlCaja);
    }
}
