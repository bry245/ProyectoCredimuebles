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



    }
}
