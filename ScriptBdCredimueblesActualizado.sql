USE [master]
GO
/****** Object:  Database [BDCredimuebles]    Script Date: 6/6/2024 19:46:21 ******/
CREATE DATABASE [BDCredimuebles]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BDCredimuebles', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\BDCredimuebles.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BDCredimuebles_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\BDCredimuebles_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [BDCredimuebles] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BDCredimuebles].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BDCredimuebles] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BDCredimuebles] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BDCredimuebles] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BDCredimuebles] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BDCredimuebles] SET ARITHABORT OFF 
GO
ALTER DATABASE [BDCredimuebles] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [BDCredimuebles] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BDCredimuebles] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BDCredimuebles] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BDCredimuebles] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BDCredimuebles] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BDCredimuebles] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BDCredimuebles] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BDCredimuebles] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BDCredimuebles] SET  ENABLE_BROKER 
GO
ALTER DATABASE [BDCredimuebles] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BDCredimuebles] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BDCredimuebles] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BDCredimuebles] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BDCredimuebles] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BDCredimuebles] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [BDCredimuebles] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BDCredimuebles] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [BDCredimuebles] SET  MULTI_USER 
GO
ALTER DATABASE [BDCredimuebles] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BDCredimuebles] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BDCredimuebles] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BDCredimuebles] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [BDCredimuebles] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [BDCredimuebles] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [BDCredimuebles] SET QUERY_STORE = ON
GO
ALTER DATABASE [BDCredimuebles] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [BDCredimuebles]
GO
/****** Object:  Table [dbo].[BitacoraProducto]    Script Date: 6/6/2024 19:46:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BitacoraProducto](
	[idBProducto] [bigint] IDENTITY(1,1) NOT NULL,
	[IdProducto] [bigint] NULL,
	[idUsuario] [bigint] NULL,
	[accion] [varchar](100) NOT NULL,
	[fecha] [datetime] NOT NULL,
 CONSTRAINT [PK__Bitacora__670B5F3242441178] PRIMARY KEY CLUSTERED 
(
	[idBProducto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BitacoraProveedores]    Script Date: 6/6/2024 19:46:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BitacoraProveedores](
	[idBProveedor] [bigint] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](255) NULL,
	[telefono] [varchar](20) NULL,
	[correo] [varchar](255) NULL,
	[direccion] [varchar](255) NULL,
	[accion] [varchar](50) NULL,
	[fecha] [datetime] NULL,
 CONSTRAINT [PK__Bitacora__2075B6F89E93020D] PRIMARY KEY CLUSTERED 
(
	[idBProveedor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CategoriaProducto]    Script Date: 6/6/2024 19:46:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoriaProducto](
	[idCategoria] [bigint] IDENTITY(1,1) NOT NULL,
	[descripcion] [varchar](50) NULL,
 CONSTRAINT [PK__Categori__8A3D240CF24AA476] PRIMARY KEY CLUSTERED 
(
	[idCategoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comisiones]    Script Date: 6/6/2024 19:46:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comisiones](
	[idComision] [bigint] IDENTITY(1,1) NOT NULL,
	[idSalida] [bigint] NULL,
	[fecha] [date] NULL,
	[descripcion] [varchar](255) NULL,
	[monto] [decimal](10, 2) NULL,
 CONSTRAINT [PK__Comision__12A3EDC23A16B5BE] PRIMARY KEY CLUSTERED 
(
	[idComision] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ControlCaja]    Script Date: 6/6/2024 19:46:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ControlCaja](
	[fechaID] [date] NOT NULL,
	[totalventas] [int] NULL,
	[montoVentas] [decimal](10, 2) NULL,
	[ganancia] [decimal](10, 2) NULL,
	[comisiones] [decimal](10, 2) NULL,
	[idGasto] [bigint] NULL,
	[totalCaja] [decimal](10, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[fechaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CuentasPorCobrar]    Script Date: 6/6/2024 19:46:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CuentasPorCobrar](
	[idCuenta] [bigint] IDENTITY(1,1) NOT NULL,
	[idCliente] [bigint] NOT NULL,
	[fechaCompra] [date] NOT NULL,
	[plazo] [int] NOT NULL,
	[montoPagado] [decimal](10, 2) NOT NULL,
	[fechaAbono] [date] NULL,
	[saldo] [decimal](10, 2) NOT NULL,
	[proximoPago] [date] NULL,
 CONSTRAINT [PK__CuentasP__BBC6DF3293B72670] PRIMARY KEY CLUSTERED 
(
	[idCuenta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Gastos]    Script Date: 6/6/2024 19:46:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Gastos](
	[idGasto] [bigint] IDENTITY(1,1) NOT NULL,
	[descripcion] [varchar](255) NULL,
	[monto] [decimal](10, 2) NULL,
	[fecha] [date] NULL,
 CONSTRAINT [PK__Gastos__F25CC32111B36A4A] PRIMARY KEY CLUSTERED 
(
	[idGasto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MetodosPago]    Script Date: 6/6/2024 19:46:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MetodosPago](
	[idMetodoPago] [bigint] IDENTITY(1,1) NOT NULL,
	[descripcion] [varchar](255) NULL,
	[comision] [decimal](10, 2) NULL,
	[estado] [bit] NULL,
 CONSTRAINT [PK__MetodosP__817BFC32101CBA8F] PRIMARY KEY CLUSTERED 
(
	[idMetodoPago] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Producto]    Script Date: 6/6/2024 19:46:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Producto](
	[idProducto] [bigint] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](255) NULL,
	[idCategoria] [bigint] NULL,
	[idProveedor] [bigint] NULL,
	[cantidadStock] [int] NULL,
	[costo] [decimal](10, 2) NULL,
	[estado] [bit] NULL,
 CONSTRAINT [PK__Producto__07F4A1321BC6B0C7] PRIMARY KEY CLUSTERED 
(
	[idProducto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductoSalida]    Script Date: 6/6/2024 19:46:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductoSalida](
	[idLinea] [bigint] IDENTITY(1,1) NOT NULL,
	[IdSalida] [bigint] NULL,
	[idProducto] [bigint] NULL,
	[cantidad] [int] NULL,
 CONSTRAINT [PK__Producto__4F210ABFB723F06E] PRIMARY KEY CLUSTERED 
(
	[idLinea] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Proveedor]    Script Date: 6/6/2024 19:46:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Proveedor](
	[idProveedor] [bigint] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](200) NOT NULL,
	[telefono] [varchar](20) NULL,
	[correo] [varchar](255) NULL,
	[direccion] [varchar](255) NULL,
 CONSTRAINT [PK__Proveedo__A3FA8E6BC248ADFA] PRIMARY KEY CLUSTERED 
(
	[idProveedor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 6/6/2024 19:46:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[IdRol] [bigint] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](50) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[IdRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Salidas]    Script Date: 6/6/2024 19:46:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Salidas](
	[idSalida] [bigint] IDENTITY(1,1) NOT NULL,
	[idCliente] [bigint] NULL,
	[IdProductoSalida] [bigint] NULL,
	[CantidadProductos] [int] NULL,
	[idMetodoPago] [bigint] NULL,
	[MontoDeVenta] [decimal](10, 2) NULL,
	[costoVenta] [decimal](10, 2) NULL,
	[IdVendedor] [bigint] NULL,
	[ganancia] [decimal](10, 2) NULL,
	[Fecha] [date] NULL,
 CONSTRAINT [PK__Salidas__25AFC22A47FAD7CD] PRIMARY KEY CLUSTERED 
(
	[idSalida] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 6/6/2024 19:46:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[idUsuario] [bigint] IDENTITY(1,1) NOT NULL,
	[idrol] [bigint] NULL,
	[nombre] [varchar](50) NULL,
	[apellidos] [varchar](100) NULL,
	[contrasenna] [varchar](50) NULL,
	[correo] [varchar](100) NULL,
	[telefono] [varchar](15) NULL,
	[direccion] [varchar](255) NULL,
	[estado] [bit] NULL,
	[Usuario] [varchar](50) NULL,
 CONSTRAINT [PK__Usuario__080A974337FC8558] PRIMARY KEY CLUSTERED 
(
	[idUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([IdRol], [Descripcion]) VALUES (1, N'Administrador')
INSERT [dbo].[Roles] ([IdRol], [Descripcion]) VALUES (2, N'Gerente')
INSERT [dbo].[Roles] ([IdRol], [Descripcion]) VALUES (3, N'Vendedor')
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Usuario] ON 

INSERT [dbo].[Usuario] ([idUsuario], [idrol], [nombre], [apellidos], [contrasenna], [correo], [telefono], [direccion], [estado], [Usuario]) VALUES (1, 1, N'Bryan', N'Vindas Marin', N'123456', N'vindasbry@gmail.com', N'60608080', N'Santa Ana', 1, N'Bryan24')
SET IDENTITY_INSERT [dbo].[Usuario] OFF
GO
ALTER TABLE [dbo].[BitacoraProducto]  WITH CHECK ADD  CONSTRAINT [FK_BitacoraProducto_Producto] FOREIGN KEY([IdProducto])
REFERENCES [dbo].[Producto] ([idProducto])
GO
ALTER TABLE [dbo].[BitacoraProducto] CHECK CONSTRAINT [FK_BitacoraProducto_Producto]
GO
ALTER TABLE [dbo].[BitacoraProducto]  WITH CHECK ADD  CONSTRAINT [FK_BitacoraProducto_Usuario] FOREIGN KEY([idUsuario])
REFERENCES [dbo].[Usuario] ([idUsuario])
GO
ALTER TABLE [dbo].[BitacoraProducto] CHECK CONSTRAINT [FK_BitacoraProducto_Usuario]
GO
ALTER TABLE [dbo].[Comisiones]  WITH CHECK ADD  CONSTRAINT [FK_Comisiones_Salidas] FOREIGN KEY([idSalida])
REFERENCES [dbo].[Salidas] ([idSalida])
GO
ALTER TABLE [dbo].[Comisiones] CHECK CONSTRAINT [FK_Comisiones_Salidas]
GO
ALTER TABLE [dbo].[ControlCaja]  WITH CHECK ADD  CONSTRAINT [FK_ControlCaja_Gastos] FOREIGN KEY([idGasto])
REFERENCES [dbo].[Gastos] ([idGasto])
GO
ALTER TABLE [dbo].[ControlCaja] CHECK CONSTRAINT [FK_ControlCaja_Gastos]
GO
ALTER TABLE [dbo].[CuentasPorCobrar]  WITH CHECK ADD  CONSTRAINT [FK_CuentasPorCobrar_Usuario] FOREIGN KEY([idCliente])
REFERENCES [dbo].[Usuario] ([idUsuario])
GO
ALTER TABLE [dbo].[CuentasPorCobrar] CHECK CONSTRAINT [FK_CuentasPorCobrar_Usuario]
GO
ALTER TABLE [dbo].[Producto]  WITH CHECK ADD  CONSTRAINT [FK_Producto_CategoriaProducto] FOREIGN KEY([idCategoria])
REFERENCES [dbo].[CategoriaProducto] ([idCategoria])
GO
ALTER TABLE [dbo].[Producto] CHECK CONSTRAINT [FK_Producto_CategoriaProducto]
GO
ALTER TABLE [dbo].[Producto]  WITH CHECK ADD  CONSTRAINT [FK_Producto_Proveedor] FOREIGN KEY([idProveedor])
REFERENCES [dbo].[Proveedor] ([idProveedor])
GO
ALTER TABLE [dbo].[Producto] CHECK CONSTRAINT [FK_Producto_Proveedor]
GO
ALTER TABLE [dbo].[ProductoSalida]  WITH CHECK ADD  CONSTRAINT [FK_ProductoSalida_Producto] FOREIGN KEY([idProducto])
REFERENCES [dbo].[Producto] ([idProducto])
GO
ALTER TABLE [dbo].[ProductoSalida] CHECK CONSTRAINT [FK_ProductoSalida_Producto]
GO
ALTER TABLE [dbo].[ProductoSalida]  WITH CHECK ADD  CONSTRAINT [FK_ProductoSalida_Salidas] FOREIGN KEY([IdSalida])
REFERENCES [dbo].[Salidas] ([idSalida])
GO
ALTER TABLE [dbo].[ProductoSalida] CHECK CONSTRAINT [FK_ProductoSalida_Salidas]
GO
ALTER TABLE [dbo].[Salidas]  WITH CHECK ADD  CONSTRAINT [FK_Salidas_MetodosPago] FOREIGN KEY([idMetodoPago])
REFERENCES [dbo].[MetodosPago] ([idMetodoPago])
GO
ALTER TABLE [dbo].[Salidas] CHECK CONSTRAINT [FK_Salidas_MetodosPago]
GO
ALTER TABLE [dbo].[Salidas]  WITH CHECK ADD  CONSTRAINT [FK_Salidas_Usuario] FOREIGN KEY([idCliente])
REFERENCES [dbo].[Usuario] ([idUsuario])
GO
ALTER TABLE [dbo].[Salidas] CHECK CONSTRAINT [FK_Salidas_Usuario]
GO
ALTER TABLE [dbo].[Salidas]  WITH CHECK ADD  CONSTRAINT [FK_Salidas_Usuario1] FOREIGN KEY([IdVendedor])
REFERENCES [dbo].[Usuario] ([idUsuario])
GO
ALTER TABLE [dbo].[Salidas] CHECK CONSTRAINT [FK_Salidas_Usuario1]
GO
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_Roles] FOREIGN KEY([idrol])
REFERENCES [dbo].[Roles] ([IdRol])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Roles]
GO
/****** Object:  StoredProcedure [dbo].[consultarCorreo]    Script Date: 6/6/2024 19:46:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[consultarCorreo]
@correo varchar(150)
as
begin
	select idUsuario, nombre
	from usuario
	where usuario.correo=@correo and usuario.estado=1
end
GO
/****** Object:  StoredProcedure [dbo].[IniciarSesion]    Script Date: 6/6/2024 19:46:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[IniciarSesion]
@correo varchar(150),
@contrasenna varchar(150)
as
begin
	select U.idUsuario, u.usuario ,
	r.Descripcion
	from Usuario U, Roles R
	where U.contrasenna = @contrasenna and (U.usuario=@correo or U.correo=@correo) and U.estado=1 and r.IdRol=U.idrol
end
GO
USE [master]
GO
ALTER DATABASE [BDCredimuebles] SET  READ_WRITE 
GO
