using CrediV1_Prueba.Entities;

namespace CrediV1_Prueba.Interfaces
{
	public interface IChatModel
	{
		public Task<List<ProductoDB>>? TraerDatosDB();
	}
}
