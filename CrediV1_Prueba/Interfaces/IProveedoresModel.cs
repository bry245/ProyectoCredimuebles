using CrediV1_Prueba.Entities;
using static CrediV1_Prueba.Entities.ProveedorEnt;



namespace CrediV1_Prueba.Interfaces
{
    public interface IProveedoresModel
    {
        public Task<bool> AddProveedor(ProveedorEnt proveedor);


        public Task UpdateProveedor(ProveedorEnt proveedor);

        public Task<IEnumerable<ProveedorEnt>> GetProveedores();

        public Task<ProveedorEnt> GetProveedoresID(int idProveedor);

        public Task SearchProveedorByEmail(ProveedorEnt proveedor);


        public Task DesactivarProveedor(ProveedorEnt proveedor);




    }
}
