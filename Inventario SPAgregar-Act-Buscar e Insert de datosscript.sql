
/****** Object:  StoredProcedure [dbo].[AgregarProducto]    Script Date: 7/4/2024 9:58:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[AgregarProducto]
    @nombre VARCHAR(255),
    @idCategoria BIGINT,
    @idProveedor BIGINT,
    @cantidadStock INT,
    @costo DECIMAL(18, 2)
AS
BEGIN


    INSERT INTO [BDCredimuebles].[dbo].[Producto] (nombre, idCategoria, idProveedor, cantidadStock, costo, estado)
    VALUES (@nombre, @idCategoria, @idProveedor, @cantidadStock, @costo, 1);

END
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*Insertar datos iniciales a la base de datos*/
INSERT INTO [BDCredimuebles].[dbo].[CategoriaProducto] ([descripcion])
VALUES 
    ('Juegos de sala'),
    ('Butacas'),
    ('Reclinables'),
    ('Muebles de cocina'),
    ('Roperos'),
    ('Muebles TV'),
    ('Mesas de centro'),
    ('Juegos de comedor'),
    ('Sillas'),
    ('Decorativos'),
    ('Comodas'),
    ('Camas'),
    ('Colchones'),
    ('Oficina');


INSERT INTO [BDCredimuebles].[dbo].[Producto] 
    ( [idProducto], [nombre], [idCategoria], [idProveedor], [costo], [cantidadStock], [estado])
VALUES
    (1, 'Alacena 2 puertas', 'Muebles de cocina', 'Daniel Quiros', 150000, 15, 'Activo'),
    (2, 'Sofa reclinable', 'Reclinables', 'Bismar', 200000, 5, 'Activo'),
    (3, 'Cama king', 'Camas', 'Alfonso', 205000, 0, 'Inactivo'),
    (1, 'Silla oficina', 'Oficina', 'Daniel Quiros', 25000, 2, 'Activo'),
    (2, 'Mueble televisor', 'Mueble TV', 'Bismar', 38000, 10, 'Activo');

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
CREATE PROCEDURE [dbo].[ActualizarProducto]
    @nombre VARCHAR(255),
    @idCategoria BIGINT,
	@idProducto BIGINT,
    @idProveedor BIGINT,
    @cantidadStock INT,
    @costo DECIMAL(18, 2)
AS
BEGIN

    UPDATE [BDCredimuebles].[dbo].[Producto]
    SET nombre = @nombre,
        idCategoria = @idCategoria,
        idProveedor = @idProveedor,
        cantidadStock = @cantidadStock,
        costo = @costo
    WHERE idProducto = @idProducto;
END
GO
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
USE [BDCredimuebles]
GO
/****** Object:  StoredProcedure [dbo].[BuscarProducto]    Script Date: 7/4/2024 7:41:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[BuscarProducto]

    @id INT
AS
BEGIN


   select prod.idProducto,prod.nombre, c.descripcion as CategoriaNombre, p.nombre as proveedor_nombre, cantidadStock, costo, prod.estado, prod.idCategoria, prod.idProveedor
   from  [BDCredimuebles].[dbo].[Producto] prod
   inner join [BDCredimuebles].[dbo].CategoriaProducto c
   on prod.idCategoria=c.idCategoria
   inner join Proveedor p
   on prod.idProveedor=p.idProveedor
    where prod.idProducto= @id;

END

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

