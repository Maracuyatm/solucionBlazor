CREATE DATABASE DBCrudBlazor;
GO

USE DBCrudBlazor;
GO

CREATE TABLE [dbo].[tipo_activo] (
    [id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [nombre] VARCHAR(50) NOT NULL,
    [created_at] DATETIME2(7) NULL,
    [updated_at] DATETIME2(7) NULL,
    [deleted_at] DATETIME2(7) NULL
);
GO

CREATE TABLE [dbo].[marca] (
    [id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [tipo_activo_id] UNIQUEIDENTIFIER NOT NULL,
    [nombre] VARCHAR(50) NOT NULL,
    [created_at] DATETIME2(7) NULL,
    [updated_at] DATETIME2(7) NULL,
    [deleted_at] DATETIME2(7) NULL,
    [estado] SMALLINT NULL,
    CONSTRAINT FK_marca_tipo_activo FOREIGN KEY (tipo_activo_id) REFERENCES tipo_activo(id)
);
GO

CREATE TABLE [dbo].[sistema_operativo] (
    [id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [tipo_activo_id] UNIQUEIDENTIFIER NOT NULL,
    [nombre] VARCHAR(50) NOT NULL,
    [created_at] DATETIME2(7) NULL,
    [updated_at] DATETIME2(7) NULL,
    [deleted_at] DATETIME2(7) NULL,
    [estado] SMALLINT NOT NULL,
    CONSTRAINT FK_sistema_operativo_tipo_activo FOREIGN KEY (tipo_activo_id) REFERENCES tipo_activo(id)
);
GO

CREATE TABLE [dbo].[procesador] (
    [id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [tipo_activo_id] UNIQUEIDENTIFIER NOT NULL,
    [nombre] VARCHAR(50) NOT NULL,
    [created_at] DATETIME2(7) NULL,
    [updated_at] DATETIME2(7) NULL,
    [deleted_at] DATETIME2(7) NULL,
    [estado] SMALLINT NOT NULL,
    CONSTRAINT FK_procesador_tipo_activo FOREIGN KEY (tipo_activo_id) REFERENCES tipo_activo(id)
);
GO

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
    [deleted_at] DATETIME2(7) NULL,
    CONSTRAINT FK_activo_tipo_activo FOREIGN KEY (tipo_activo_id) REFERENCES tipo_activo(id),
    CONSTRAINT FK_activo_marca FOREIGN KEY (marca_id) REFERENCES marca(id),
    CONSTRAINT FK_activo_estado FOREIGN KEY (estado_id) REFERENCES estado(id), -- asumo que existe tabla estado
    CONSTRAINT FK_activo_sistema_operativo FOREIGN KEY (sistema_operativo_id) REFERENCES sistema_operativo(id),
    CONSTRAINT FK_activo_procesador FOREIGN KEY (procesador) REFERENCES procesador(id)
);
GO
