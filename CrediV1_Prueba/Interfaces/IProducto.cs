﻿using CrediV1_Prueba.Entities;

namespace CrediV1_Prueba.Interfaces
{
	public interface IProducto
	{

		public Task<IEnumerable<ProductoEnt>> GetProductos();
        public Task<bool> agregarProducto(ProductoEnt producto);

        public  Task<string> DesactivarProducto(ProductoEnt idProducto);
	}
}
