namespace CrediV1_Prueba.Interfaces;
using CrediV1_Prueba.Models;
using CrediV1_Prueba.Entities;
using CrediV1_Prueba.Entities.Otros;
{
    public interface IGastosModel
    {
        Task<bool> AgregarGasto(Gasto gasto);
    }
}
