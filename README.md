# Taller 2: ORBITA


## 1. Integrantes del equipo:
- Guerrero Polanco Jose Manuel CP240499
- Fernando Josue Quintanilla Avalos QA221370
- Escobar Ortega Emmanuel Alexander EO260404

## 2. Instrucciones de ejecución.
### Requisitos Previos:
- Visual Studio 2019 / 2022 con soporte para desarrollo de escritorio .NET (.NET Framework 4.7.2).
- Microsoft SQL Server Express / LocalDB instalado y corriendo.

### Configuración de la Base de Datos:
- Ejecutar el script SQL adjunto (ORBITA_DB_Script.sql) en SQL Server Management Studio (SSMS) o Visual Studio para crear la base de datos ORBITA_DB y las tablas correspondientes (Usuarios, Misiones, Recursos, Asignaciones).
- Verificar la cadena de conexión (ConnectionString) en el archivo de configuración del proyecto ORBITA.Datos (ConexionBD.cs).
### Compilación y Ejecución:
- Abrir el archivo de solución ORBITA.sln en Visual Studio.
- Establecer el proyecto ORBITA.App como Proyecto de Inicio (Set as Startup Project).
- Compilar la solución (Ctrl + Shift + B) y ejecutar (F5 o Ctrl + F5).

## 3. Credenciales de prueba.

## 4. Distribución general de módulos.
La solución está estructurada arquitectónicamente en 3 proyectos diferenciados:

### 1. ORBITA.Dominio (Biblioteca de Clases):
- Jerarquía de Recursos: RecursoExploracion (Abstracta), Dron, RoverTerrestre, EstacionSensores.
- Entidades: Usuario, Mision, AsignacionRecurso.
- Interfaces: IAsignable.
- Excepciones & Enums: RecursoNoDisponibleException, EstadoRecurso, EstadoMision, RolUsuario.

### 2. ORBITA.Datos (Biblioteca de Clases):
- Conexión: ConexionBD (Gestión de ADO.NET / Connection String).
- Repositorios: UsuarioRepository, RecursoRepository, MisionRepository.

### 3. ORBITA.App (Aplicación de Consola):
- Flujo y Control: Program.cs (Menús de interacción, inicio/cierre de sesión).
- Servicios & Reglas: SesionService, ProtocoloSeguridad (Validación de reglas pre-inicio de misión).
## 5. Principios SOLID identificados.
Se han identificado e implementado al menos dos principios SOLID en el diseño de la solución:

### 1. Single Responsibility Principle (SRP) / Principio de Responsabilidad Única:
- Dónde se identifica: Separación clara entre MisionRepository (Capa de Datos, encargada únicamente de la persistencia SQL) y ProtocoloSeguridad (Capa de Dominio/App, encargada únicamente de validar las 6 reglas de negocio antes de iniciar una misión).
- Problema que evita: Evita acoplar la lógica de acceso a la base de datos con las reglas del negocio, facilitando el mantenimiento y las pruebas unitarias.

### 2. Open/Closed Principle (OCP) / Principio de Abierto/Cerrado:
- Dónde se identifica: Clase base abstracta RecursoExploracion y el método polimórfico CalcularCostoOperacion().
- Problema que evita: Permite agregar nuevos tipos de recursos (ej. Satelite, Submarino) heredando de RecursoExploracion sin modificar el código existente en las vistas o repositorios ni requerir bloqueos de lógica tipo if/switch.
## 6. Patrón de diseño propuesto.
### Nombre del Patrón:
Factory Method (Fábrica).
### Clasificación:
Creacional.
### Problema que resolvería:
En versiones futuras, la creación manual e instanciación de distintos tipos de recursos (Dron, RoverTerrestre, EstacionSensores) desde los repositorios o controladores puede volverse compleja. El patrón Factory Method encapsularía la lógica de instanciación según el tipo de recurso almacenado en la base de datos.
### Clases Participantes:
- RecursoFactory (Clase abstracta/interfaz creadora).
- DronFactory, RoverFactory, SensoresFactory (Creadores concretos).
- RecursoExploracion (Producto abstracto).
# 7. Evidencia de las pruebas realizadas.
