IF DB_ID(N'TallerMecanico') IS NULL
    CREATE DATABASE TallerMecanico;
GO
USE TallerMecanico;
GO

IF OBJECT_ID(N'dbo.Clientes', N'U') IS NULL
CREATE TABLE dbo.Clientes
(
    Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Clientes PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Telefono NVARCHAR(20) NOT NULL
);

IF OBJECT_ID(N'dbo.OrdenesServicio', N'U') IS NULL
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
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Clientes)
    INSERT dbo.Clientes (Nombre, Telefono) VALUES
    (N'María Quispe', N'987654321'),
    (N'Carlos Mendoza', N'912345678'),
    (N'Lucía Salazar', N'956781234');

IF NOT EXISTS (SELECT 1 FROM dbo.OrdenesServicio)
    INSERT dbo.OrdenesServicio (Placa, DescripcionTrabajo, FechaIngreso, Estado, ClienteId) VALUES
    (N'ABC-123', N'Cambio de aceite y filtro', '2026-11-10', N'Pendiente', 1),
    (N'BDE-456', N'Revisión de frenos delanteros', '2026-11-09', N'En proceso', 2),
    (N'FJK-789', N'Alineamiento y balanceo', '2026-11-08', N'Completada', 1);
GO
