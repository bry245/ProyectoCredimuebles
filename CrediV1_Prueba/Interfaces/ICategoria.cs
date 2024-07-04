using CrediV1_Prueba.Entities;

namespace CrediV1_Prueba.Interfaces
{
    public interface ICategoria
    {

        public Task<IEnumerable<CategoriaEnt>> GetCategorias();
    }
}
