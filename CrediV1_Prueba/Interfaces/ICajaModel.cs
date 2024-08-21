using CrediV1_Prueba.Entities;
using X.PagedList;

namespace CrediV1_Prueba.Interfaces
{
    public interface ICajaModel
    {
        public Task<IPagedList<CajaEnt>> ListarCajasDiarias(int paginas, int tamaño);
        public Task<IPagedList<CajaEnt>> ListarGastos(int paginas, int tamaño, DateTime fecha);
        public long RegistrarGasto(CajaEnt Gasto);
        public int EliminarGasto(CajaEnt datos);
        public CajaEnt ObtenerDatosCaja();
    }
}
