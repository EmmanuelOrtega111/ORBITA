-- 1. Crear la base de datos
CREATE DATABASE ORBITAControlDB;
GO

USE ORBITAControlDB;
GO

-- 2. Tabla Usuarios
CREATE TABLE Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NombreUsuario VARCHAR(50) NOT NULL UNIQUE,
    Contrasena VARCHAR(100) NOT NULL,
    Rol VARCHAR(30) NOT NULL, -- 'Administrador', 'Coordinador', 'Auditor'
    Activo BIT NOT NULL DEFAULT 1
);

-- 3. Tabla Misiones
CREATE TABLE Misiones (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Codigo VARCHAR(20) NOT NULL UNIQUE,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(255) NULL,
    Prioridad INT NOT NULL,
    FechaInicio DATETIME NOT NULL,
    FechaEstimadaFin DATETIME NOT NULL,
    Estado VARCHAR(30) NOT NULL, -- 'Planificada', 'EnEjecucion', 'Finalizada'
    IdResponsable INT NOT NULL,
    FOREIGN KEY (IdResponsable) REFERENCES Usuarios(Id)
);

-- 4. Tabla Recursos (Maneja la herencia Polimórfica)
CREATE TABLE Recursos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Codigo VARCHAR(20) NOT NULL UNIQUE,
    Modelo VARCHAR(100) NOT NULL,
    Tipo VARCHAR(30) NOT NULL, -- 'Dron', 'Rover', 'EstacionSensores'
    Estado VARCHAR(30) NOT NULL, -- 'Disponible', 'Asignado', 'Mantenimiento'
    
    -- Atributos específicos según tipo (pueden ser NULL)
    AutonomiaVuelo FLOAT NULL,      -- Dron
    Alcance FLOAT NULL,             -- Dron
    Autonomia FLOAT NULL,           -- Rover
    CapacidadCarga FLOAT NULL,      -- Rover
    CantidadSensores INT NULL,      -- EstacionSensores
    ConsumoEnergetico FLOAT NULL,   -- EstacionSensores
    
    CostoBase DECIMAL(18,2) NOT NULL -- Costo por hora / km / día
);

-- 5. Tabla Asignaciones (Relación Misión - Recurso)
CREATE TABLE Asignaciones (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IdMision INT NOT NULL,
    IdRecurso INT NOT NULL,
    FechaAsignacion DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (IdMision) REFERENCES Misiones(Id),
    FOREIGN KEY (IdRecurso) REFERENCES Recursos(Id)
);
GO