

using CrediV1_Prueba.Models;

namespace CrediV1_Prueba.Services.Proveedores

{
    public interface IProveedores
    {
        public Task AddProveedores(Proveedor proveedor);
        public  Task<IEnumerable<Proveedor>> GetProveedores();

        public  Task<Proveedor> GetProveedoresID(int idProveedor);

        public Task UpdateProveedor(Proveedor proveedor);



	}
}
