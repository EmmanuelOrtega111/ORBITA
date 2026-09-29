USE ORBITAControlDB;
GO

-- 1. Insertar 3 Usuarios con roles distintos (Punto 19)
INSERT INTO Usuarios (NombreUsuario, Contrasena, Rol, Activo) VALUES 
('admin_user', 'admin123', 'Administrador', 1),
('coord_user', 'coord123', 'Coordinador', 1),
('audit_user', 'audit123', 'Auditor', 1);

-- 2. Insertar 3 Drones, 3 Rovers y 3 Estaciones (al menos 1 en Mantenimiento) (Punto 19)
-- Drones
INSERT INTO Recursos (Codigo, Modelo, Tipo, Estado, AutonomiaVuelo, Alcance, CostoBase) VALUES 
('DRN-001', 'Falcon X1', 'Dron', 'Disponible', 45.0, 15.0, 120.00),
('DRN-002', 'SkyGuardian', 'Dron', 'Asignado', 60.0, 25.0, 150.00),
('DRN-003', 'Scout Pro', 'Dron', 'Mantenimiento', 30.0, 10.0, 90.00); -- Recurso en Mantenimiento

-- Rovers
INSERT INTO Recursos (Codigo, Modelo, Tipo, Estado, Autonomia, CapacidadCarga, CostoBase) VALUES 
('RVR-001', 'Curiosity II', 'Rover', 'Disponible', 120.0, 50.0, 80.00),
('RVR-002', 'Titan Explorer', 'Rover', 'Asignado', 200.0, 100.0, 110.00),
('RVR-003', 'Mars Tracker', 'Rover', 'Disponible', 90.0, 30.0, 75.00);

-- Estaciones de Sensores
INSERT INTO Recursos (Codigo, Modelo, Tipo, Estado, CantidadSensores, ConsumoEnergetico, CostoBase) VALUES 
('STN-001', 'Atmosphere Alpha', 'EstacionSensores', 'Disponible', 8, 15.5, 200.00),
('STN-002', 'Seismic Beta', 'EstacionSensores', 'Asignado', 12, 22.0, 250.00),
('STN-003', 'Weather Gamma', 'EstacionSensores', 'Disponible', 5, 10.0, 150.00);

-- 3. Insertar 4 Misiones (Planificada, En Ejecución, Finalizada) (Punto 19)
INSERT INTO Misiones (Codigo, Nombre, Descripcion, Prioridad, FechaInicio, FechaEstimadaFin, Estado, IdResponsable) VALUES 
('MIS-001', 'Exploración Cráter Norte', 'Análisis topográfico', 1, '2026-10-01', '2026-10-15', 'Planificada', 2),
('MIS-002', 'Mapeo Atmosférico', 'Escaneo de gases', 2, '2026-09-20', '2026-10-05', 'EnEjecucion', 2),
('MIS-003', 'Reconocimiento Mineral', 'Muestreo de suelo', 3, '2026-08-01', '2026-08-30', 'Finalizada', 2),
('MIS-004', 'Monitoreo Sísimico', 'Registro de vibraciones', 2, '2026-11-01', '2026-11-20', 'Planificada', 2);

-- 4. Asignaciones de la Misión en Ejecución y Finalizada
INSERT INTO Asignaciones (IdMision, IdRecurso) VALUES 
(2, 2), -- Mision 2 tiene asignado DRN-002
(2, 5), -- Mision 2 tiene asignado RVR-002
(2, 8), -- Mision 2 tiene asignado STN-002
(3, 1); -- Mision 3 tuvo asignado DRN-001
GO

SELECT * FROM Recursos;

SELECT * FROM Misiones;

ALTER TABLE Misiones
ADD CostoEstimadoOperacion DECIMAL(18, 2) NULL DEFAULT 0.00;