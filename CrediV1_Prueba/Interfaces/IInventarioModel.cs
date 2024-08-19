using CrediV1_Prueba.Entities;
using CrediV1_Prueba.Entities.DTO;
using CrediV1_Prueba.Entities.Otros;

namespace CrediV1_Prueba.Interfaces
{
    public interface IInventarioModel
    {

        public Task<IEnumerable<ProductosBajoStock>> ConsultarProductosBajosStock();

        public Task<IEnumerable<ProductosBajoStock>> ConsultarRecomendacionestock();

        public Task<IEnumerable<PedidoEnt>> ConsultarPedidos();
        public Task<IEnumerable<PedidoEnt>> ConsultarPedidosDetalles();
        public Task<IEnumerable<PedidoEnt>> ConsultarPedidoDetallesPorID(long id);

        public int RegistrarPedido(RegistrarPedidoDTO ent);
        public void RegistrarPedidoDetalle(RegistrarPedidoDTO ent);

        public void ActualizarPedido(RegistrarPedidoDTO ent);


        public  Task<bool> RestablecerStockProducto(RestablecerCantidadDTO ent);
        public Task<string> ConfirmarPedido(RegistrarPedidoDTO ent);

    }
}