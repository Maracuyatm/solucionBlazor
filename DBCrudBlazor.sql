-- Eliminar la base de datos si ya existe
IF DB_ID('DBCrudBlazor') IS NOT NULL
BEGIN
    ALTER DATABASE DBCrudBlazor SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE DBCrudBlazor;
END
GO

-- Crear la base de datos
CREATE DATABASE DBCrudBlazor;
GO

-- Usar la base de datos recién creada
USE DBCrudBlazor;
GO

-- Tabla tipo_activo
CREATE TABLE [dbo].[tipo_activo] (
    [id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [nombre] VARCHAR(50) NOT NULL,
    [created_at] DATETIME2(7) NULL,
    [updated_at] DATETIME2(7) NULL,
    [deleted_at] DATETIME2(7) NULL
);
GO

-- Tabla marca
CREATE TABLE [dbo].[marca] (
    [id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [tipo_activo_id] UNIQUEIDENTIFIER NOT NULL,
    [nombre] VARCHAR(50) NOT NULL,
    [created_at] DATETIME2(7) NULL,
    [updated_at] DATETIME2(7) NULL,
    [deleted_at] DATETIME2(7) NULL,
    [estado] SMALLINT NULL
);
GO

-- Tabla sistema_operativo
CREATE TABLE [dbo].[sistema_operativo] (
    [id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [tipo_activo_id] UNIQUEIDENTIFIER NOT NULL,
    [nombre] VARCHAR(50) NOT NULL,
    [created_at] DATETIME2(7) NULL,
    [updated_at] DATETIME2(7) NULL,
    [deleted_at] DATETIME2(7) NULL,
    [estado] SMALLINT NOT NULL
);
GO

-- Tabla procesador
CREATE TABLE [dbo].[procesador] (
    [id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [tipo_activo_id] UNIQUEIDENTIFIER NOT NULL,
    [nombre] VARCHAR(50) NOT NULL,
    [created_at] DATETIME2(7) NULL,
    [updated_at] DATETIME2(7) NULL,
    [deleted_at] DATETIME2(7) NULL,
    [estado] SMALLINT NOT NULL
);
GO

-- Tabla activo
CREATE TABLE [dbo].[activo] (
    [id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [tipo_activo_id] UNIQUEIDENTIFIER NOT NULL,
    [marca_id] UNIQUEIDENTIFIER NOT NULL,
    [estado_id] UNIQUEIDENTIFIER NOT NULL,
    [serial] VARCHAR(50) NOT NULL,
    [sistema_operativo_id] UNIQUEIDENTIFIER NULL,
    [modelo] VARCHAR(50) NULL,
    [procesador] UNIQUEIDENTIFIER NULL,
    [nombre] VARCHAR(250) NOT NULL,
    [numero] SMALLINT NULL,
    [ram] SMALLINT NULL,
    [almacenamiento] SMALLINT NULL,
    [fecha_adquisicion] DATETIME2(7) NOT NULL,
    [created_at] DATETIME2(7) NULL,
    [updated_at] DATETIME2(7) NULL,
    [deleted_at] DATETIME2(7) NULL
);
GO
