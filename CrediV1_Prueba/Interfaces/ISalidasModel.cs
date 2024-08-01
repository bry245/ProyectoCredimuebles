using CrediV1_Prueba.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace CrediV1_Prueba.Interfaces
{
    public interface ISalidasModel
    {
        public Task<IPagedList<SalidasEnt>> ListarSalidas(int pageNumber, int pageSize);

        public List<SelectListItem>? ConsultarVendedores();
        public List<SelectListItem>? ConsultarMetodosPago();
        public List<ProductoEnt> ObtenerProductos();
        public long RegistrarSalida(SalidasEnt salida);
        public   Task<bool> RegistrarProductosSalida(ProductoEnt producto);
        public SalidasEnt VerSalida(long idSalida);
        public List<ProductoEnt> ObtenerProductoDeSalida(long idsalida);
        public long ActualizarSalida(SalidasEnt salida);
        public Task<bool> ActualizarProductosSalida(ProductoEnt producto);
        public int ActualizarInventarioEditarSalida(ProductoEnt producto);
        public Task<IPagedList<SalidasEnt>> ConsultarSalidasOrdenadas(int paginas, int tamaño);
        public int AnularSalida(long q);
        public long RegistrarCuentaCredito(SalidasEnt salida);
        public  Task<IPagedList<SalidasEnt>> ListarCuentasPorCobrar(int paginas, int tamaño);
        public SalidasEnt VerCuentaPorCobrar(long idCuenta);
        public List<SalidasEnt> ObtenerPagosRealizados(long idCuenta);
        public int AgregarAbono(SalidasEnt cuenta);






    }
}
