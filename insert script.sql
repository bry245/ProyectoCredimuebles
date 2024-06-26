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
