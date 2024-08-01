using CrediV1_Prueba.Entities.DTO;
using CrediV1_Prueba.Entities.Otros;

namespace CrediV1_Prueba.Interfaces
{
    public interface IInventarioModel
    {

        public Task<IEnumerable<ProductosBajoStock>> ConsultarProductosBajosStock();

        public Task<IEnumerable<ProductosBajoStock>> ConsultarRecomendacionestock();


        public  Task<bool> RestablecerStockProducto(RestablecerCantidadDTO ent);

    }
}