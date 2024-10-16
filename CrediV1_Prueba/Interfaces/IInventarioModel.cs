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
        public Task<IEnumerable<UsuarioEnt>> ConsultarCorreosAdministradores();

        public int RegistrarPedido(RegistrarPedidoDTO ent);
        public void RegistrarPedidoDetalle(RegistrarPedidoDTO ent);


        public  Task<IEnumerable<PedidoEnt>> ConsultarPedidosDetallesEnCurso();
        public void ActualizarPedido(RegistrarPedidoDTO ent);
        public  Task<bool> RestablecerStockProducto(RestablecerCantidadDTO ent);
        public Task<string> ConfirmarPedido(RegistrarPedidoDTO ent);
        public  void CancelarPedido(RegistrarPedidoDTO ent);


        public void RegistrarBitacoraProducto(BitacoraProducto ent);
         public void RegistrarBitacoraProveedor(BitacoraProveedor ent);



        public Task<IEnumerable<BitacoraProducto>> ConsultarBitacoraProductos();

        public Task<IEnumerable<BitacoraProveedor>> ConsultarBitacoraProveedores();




    }
}