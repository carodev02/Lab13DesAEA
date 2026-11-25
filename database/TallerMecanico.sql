IF DB_ID(N'TallerMecanico') IS NULL
    CREATE DATABASE TallerMecanico;
GO

USE TallerMecanico;
GO

IF OBJECT_ID(N'dbo.Clientes', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Clientes
    (
        Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Clientes PRIMARY KEY,
        Nombre NVARCHAR(100) NOT NULL,
        Telefono NVARCHAR(20) NOT NULL
    );
END;
GO

IF OBJECT_ID(N'dbo.OrdenesServicio', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.OrdenesServicio
    (
        Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_OrdenesServicio PRIMARY KEY,
        Placa NVARCHAR(10) NOT NULL,
        DescripcionTrabajo NVARCHAR(300) NOT NULL,
        FechaIngreso DATE NOT NULL,
        Estado NVARCHAR(20) NOT NULL,
        ClienteId INT NOT NULL,
        CONSTRAINT FK_OrdenesServicio_Clientes FOREIGN KEY (ClienteId)
            REFERENCES dbo.Clientes(Id) ON DELETE NO ACTION
    );
END;
GO

IF OBJECT_ID(N'dbo.Usuarios', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Usuarios
    (
        Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Usuarios PRIMARY KEY,
        Correo NVARCHAR(254) NOT NULL,
        ClaveHash NVARCHAR(100) NOT NULL,
        Rol NVARCHAR(30) NOT NULL,
        CONSTRAINT UQ_Usuarios_Correo UNIQUE (Correo),
        CONSTRAINT CK_Usuarios_Rol CHECK (Rol IN (N'Cliente', N'Recepcionista', N'Mecanico'))
    );
END;
GO

-- Datos de demostración: se insertan individualmente para permitir reejecutar el script.
IF NOT EXISTS (SELECT 1 FROM dbo.Clientes WHERE Nombre = N'María Quispe')
    INSERT dbo.Clientes (Nombre, Telefono) VALUES (N'María Quispe', N'987654321');
IF NOT EXISTS (SELECT 1 FROM dbo.Clientes WHERE Nombre = N'Carlos Mendoza')
    INSERT dbo.Clientes (Nombre, Telefono) VALUES (N'Carlos Mendoza', N'912345678');
IF NOT EXISTS (SELECT 1 FROM dbo.Clientes WHERE Nombre = N'Lucía Salazar')
    INSERT dbo.Clientes (Nombre, Telefono) VALUES (N'Lucía Salazar', N'956781234');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.OrdenesServicio WHERE Placa = N'ABC-123')
    INSERT dbo.OrdenesServicio (Placa, DescripcionTrabajo, FechaIngreso, Estado, ClienteId)
    VALUES (N'ABC-123', N'Cambio de aceite y filtro', '2026-11-10', N'Pendiente',
        (SELECT TOP (1) Id FROM dbo.Clientes WHERE Nombre = N'María Quispe'));
IF NOT EXISTS (SELECT 1 FROM dbo.OrdenesServicio WHERE Placa = N'BDE-456')
    INSERT dbo.OrdenesServicio (Placa, DescripcionTrabajo, FechaIngreso, Estado, ClienteId)
    VALUES (N'BDE-456', N'Revisión de frenos delanteros', '2026-11-09', N'En proceso',
        (SELECT TOP (1) Id FROM dbo.Clientes WHERE Nombre = N'Carlos Mendoza'));
IF NOT EXISTS (SELECT 1 FROM dbo.OrdenesServicio WHERE Placa = N'FJK-789')
    INSERT dbo.OrdenesServicio (Placa, DescripcionTrabajo, FechaIngreso, Estado, ClienteId)
    VALUES (N'FJK-789', N'Alineamiento y balanceo', '2026-11-08', N'Completada',
        (SELECT TOP (1) Id FROM dbo.Clientes WHERE Nombre = N'María Quispe'));
GO

-- Filas de referencia con hashes de claves aleatorias desconocidas; no hay contraseñas en texto claro.
-- Para autenticarse, registre sus propias cuentas desde la API y asígneles roles en local.
IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE Correo = N'cliente@tallermecanico.test')
    INSERT dbo.Usuarios (Correo, ClaveHash, Rol) VALUES
    (N'cliente@tallermecanico.test', N'$2a$12$/r6sZ155aNKlRf.HlfJ3xOuYh3f.IL0.wEViT19aPzd2fOCxAWj6y', N'Cliente');
IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE Correo = N'recepcionista@tallermecanico.test')
    INSERT dbo.Usuarios (Correo, ClaveHash, Rol) VALUES
    (N'recepcionista@tallermecanico.test', N'$2a$12$i/QEBolXNqWv6Z7ETXZUPetn9JsP1Mc0yQNgNiicHQ.UCh.g3yoX.', N'Recepcionista');
IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE Correo = N'mecanico@tallermecanico.test')
    INSERT dbo.Usuarios (Correo, ClaveHash, Rol) VALUES
    (N'mecanico@tallermecanico.test', N'$2a$12$dlgAbMpP21OyBoULqfDy1.9ODZscI0DL3V3rZLcIqOxfUqpwvQ1IW', N'Mecanico');
GO

-- Para asignar un rol de personal tras registrar una cuenta pública, ejecutar por ejemplo:
-- UPDATE dbo.Usuarios SET Rol = N'Recepcionista' WHERE Correo = N'nombre@correo.test';
