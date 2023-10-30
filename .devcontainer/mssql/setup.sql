CREATE DATABASE ApplicationDB;
GO
use ApplicationDB

create table Configuracion
(
    recurso varchar(50),
    propiedad varchar(50),
    valor varchar(60)
)

create table Rol
(
    idRol int primary key identity(1,1),
    descripcion varchar(30),
    esActivo bit,
    fechaRegistro datetime default getdate()
)


create table Menu
(
    idMenu int primary key identity(1,1),
    descripcion varchar(30),
    idMenuPadre int references Menu(idMenu),
    icono varchar(30),
    controlador varchar(30),
    paginaAccion varchar(30),
    esActivo bit,
    fechaRegistro datetime default getdate()
)


create table RolMenu
(
    idRolMenu int primary key identity(1,1),
    idRol int references Rol(idRol),
    idMenu int references Menu(idMenu),
    esActivo bit,
    fechaRegistro datetime default getdate()
)


create table USUARIO
(
    UsuarioId INT PRIMARY KEY IDENTITY(1,1),
    idRol int FOREIGN KEY REFERENCES Rol(idRol),
    Correo varchar(35),
    Clave varchar(100)
)

create table Categoria
(
    CategoriaId int primary key IDENTITY(1, 1),
    Nombre varchar(100),
    Descripcion varchar(255),
    TipoMovimiento varchar(8),
)

create table GASTO
(
    GastoId INT PRIMARY KEY IDENTITY(1,1),
    UsuarioId INT FOREIGN KEY REFERENCES USUARIO(UsuarioId),
    Nombre varchar(35),
    Descripcion varchar(75),
    TipoMovimiento varchar(8),
    CategoriaId INT FOREIGN KEY REFERENCES Categoria(CategoriaId),
    Monto INT,
    FechaDeIngreso DATE
)

create table LIMITEGASTOMENSUAL
(
    UsuarioId INT FOREIGN KEY REFERENCES USUARIO(UsuarioId),
    LimiteGastoMensual INT,
)

create table HISTORIALGASTOMENSUAL
(
    UsuarioId INT FOREIGN KEY REFERENCES USUARIO(UsuarioId),
    GastoMensual INT,
    FechaFinMes DATE
)

create table HISTORIALCOMENTARIOS
(
    ComentarioId INT PRIMARY KEY IDENTITY(1,1),
    UsuarioId INT FOREIGN KEY REFERENCES USUARIO(UsuarioId),
    Comentario varchar(150),
    Calificacion INT
)

