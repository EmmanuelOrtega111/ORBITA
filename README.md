# 🛰️ Sistema ORBITA Control

Sistema de gestión y monitoreo para misiones de exploración y asignación de recursos tecnológicos (Drones, Rovers Terrestres y Estaciones de Sensores). Desarrollado en **C# (.NET Framework)** utilizando una arquitectura por capas con persistencia en **SQL Server (ADO.NET)**.

---

## 1. 👥 Integrantes del Equipo

* **Guerrero Polanco, José Manuel** - `CP240499`
* **Quintanilla Ávalos, Fernando Josué** - `QA221370`
* **Escobar Ortega, Emmanuel Alexander** - `EO260404`

---

## 2. 🚀 Instrucciones de Ejecución

### Requisitos Previos
* **IDE:** Microsoft Visual Studio 2019 / 2022 (con carga de trabajo *Desarrollo de escritorio de .NET*).
* **Framework:** .NET Framework 4.7.2.
* **Base de Datos:** Microsoft SQL Server / SQL Server Express / LocalDB.

### Configuración de la Base de Datos
1. Ejecute el script SQL del proyecto (`ORBITA_DB_Script.sql`) en **SQL Server Management Studio (SSMS)** o **Visual Studio** para crear la base de datos `ORBITAControlDB` y sus tablas asociadas (`Usuarios`, `Recursos`, `Misiones`, `Asignaciones`).
2. Verifique o ajuste la cadena de conexión en el archivo `ConexionBD.cs` dentro del proyecto `ORBITA.datos`:
   ```csharp
   Server=localhost;Database=ORBITAControlDB;Trusted_Connection=True;TrustServerCertificate=True;
   ```

### Compilación y Ejecución
1. Abra el archivo de solución `ORBITA.sln` en Visual Studio.
2. Establezca el proyecto **`ORBITA.app`** como **Proyecto de Inicio** (*Set as Startup Project*).
3. Compile la solución (`Ctrl + Shift + B`).
4. Inicie la aplicación (`F5` o `Ctrl + F5`).

---

## 🔑 3. Credenciales de Prueba

| Usuario | Contraseña | Rol | Permisos Principales |
| :--- | :--- | :--- | :--- |
| `admin` | `admin123` | **Administrador** | Acceso total: Gestión de usuarios, recursos y misiones. |
| `coordinador` | `coord123` | **Coordinador** | Gestión operativa de recursos, misiones y asignaciones. |
| `auditor` | `audit123` | **Auditor** | Solo lectura: Consultas e informes (operaciones bloqueadas). |

---

## 🏗️ 4. Distribución General de Módulos

La solución se encuentra dividida arquitectónicamente en tres capas / proyectos:

```text
ORBITA/
├── ORBITA.dominios/     # Capa de Dominio (Modelos, Interfaces, Enums, Excepciones)
├── ORBITA.datos/        # Capa de Acceso a Datos (Conexión BD y Repositorios ADO.NET)
└── ORBITA.app/          # Capa de Presentación (Aplicación de Consola e Interacción)
```

### 1. `ORBITA.dominios` (Biblioteca de Clases)
* **Entidades:** `Usuario`, `Mision`, `RecursoExploracion` (Clase base abstracta).
* **Especializaciones de Recursos:** `Dron`, `RoverTerrestre`, `EstacionSensores`.
* **Interfaces:** `IAsignable`.
* **Enums & Excepciones:** `RolUsuario`, `EstadoMision`, `EstadoRecurso`, `RecursoNoDisponibleException`.

### 2. `ORBITA.datos` (Biblioteca de Clases)
* **`ConexionBD`:** Manejo centralizado de conexiones ADO.NET con `SqlConnection`.
* **Repositorios:** `UsuarioRepositorio`, `RecursoRepositorio`, `MisionRepositorio`.

### 3. `ORBITA.app` (Aplicación de Consola)
* **`Program.cs`:** Menú interactivo, control de sesión, invocación polimórfica de cálculo de costos y validación del **Protocolo de Seguridad ORBITA**.

---

## 💡 5. Principios SOLID Identificados

### 1. Single Responsibility Principle (SRP) / Principio de Responsabilidad Única
* **Ubicación:** Separación entre `MisionRepositorio` (`ORBITA.datos`) y la entidad `Mision` / método `ValidarProtocoloSeguridad()` (`ORBITA.dominios`).
* **Beneficio:** `MisionRepositorio` se encarga **únicamente** de la persistencia de datos mediante consultas SQL, mientras que `Mision` gestiona la lógica interna de validación de reglas de negocio antes de iniciar la misión.

### 2. Open/Closed Principle (OCP) / Principio de Abierto/Cerrado
* **Ubicación:** Clase abstracta `RecursoExploracion` y la sobrescritura del método polimórfico `CalcularCostoOperacion(decimal)`.
* **Beneficio:** Permite extender la funcionalidad agregando nuevos tipos de recursos (ej. *Satélite*, *Submarino*) heredando de `RecursoExploracion` e implementando su propia fórmula de costo, sin modificar las clases de negocio ni requerir condicionales tipo `if` / `switch`.

---

## 📐 6. Patrón de Diseño Propuesto

* **Nombre del Patrón:** Factory Method (Fábrica Creacional).
* **Clasificación:** Creacional.
* **Problema que resolvería:** Actualmente, la instanciación de objetos `Dron`, `RoverTerrestre` y `EstacionSensores` a partir de los datos leídos de la BD se realiza mediante bloques condicionales dentro de las capas de repositorios. El patrón *Factory Method* centralizaría e independizaría esta instanciación en clases creadoras dedicadas.
* **Estructura Propuesta:**
  * `RecursoFactory` (Creador abstracto).
  * `DronFactory`, `RoverFactory`, `EstacionSensoresFactory` (Creadores concretos).
  * `RecursoExploracion` (Producto abstracto).

---

## 📸 7. Evidencia de las Pruebas Realizadas

### Pruebas de Inicio de Sesión y Control de Acceso (RBAC)
1. **Inicio de sesión exitoso:** Autenticación de un usuario registrado desde la base de datos (`admin`).
   *(Insertar Imagen Aquí)*
2. **Rechazo de credenciales:** Intento de inicio de sesión con usuario o contraseña errónea.
   *(Insertar Imagen Aquí)*
3. **Bloqueo por Rol Auditor:** Intento de creación o modificación de recursos/misiones desde la cuenta de un usuario con rol Auditor, mostrando mensaje de acceso denegado.
   *(Insertar Imagen Aquí)*

---

### Pruebas del Módulo de Recursos
4. **Registro de Recurso (Dron/Rover/Estación):** Alta exitosa de un recurso asignándole su tipo y parámetros específicos.
   *(Insertar Imagen Aquí)*
5. **Consulta de Recursos Disponibles:** Listado filtrado mostrando únicamente los recursos con estado `Disponible`.
   *(Insertar Imagen Aquí)*
6. **Cambio de Estado de Recurso:** Modificación del estado del recurso a `Mantenimiento` o `Asignado`.
   *(Insertar Imagen Aquí)*

---

### Pruebas del Módulo de Misiones y Asignación
7. **Creación de Misión:** Registro de una nueva misión en estado `Planificada`.
   *(Insertar Imagen Aquí)*
8. **Asignación de Recursos a Misión:** Enlace exitoso entre un recurso disponible y una misión activa.
   *(Insertar Imagen Aquí)*
9. **Cálculo Polimórfico de Costo Estimado:** Invocación del método `CalcularCostoOperacion()` según el tipo específico de recurso (Horas en Drones, Kilómetros en Rovers, Días en Estaciones).
   *(Insertar Imagen Aquí)*

---

### Pruebas del Protocolo de Seguridad y Excepciones
10. **Rechazo de Misión por Recurso en Mantenimiento:** Intento de iniciar una misión que contiene un recurso en estado `Mantenimiento`, activando la regla de seguridad.
    *(Insertar Imagen Aquí)*
11. **Captura de Excepción Personalizada (`RecursoNoDisponibleException`):** Demostración del manejo de excepciones personalizadas al violar las reglas de asignación o seguridad.
    *(Insertar Imagen Aquí)*
12. **Inicio Exitoso de Misión:** Misión que cumple con los 6 criterios de seguridad pasando al estado `EnEjecucion`.
    *(Insertar Imagen Aquí)*

---

### Dashboard General
13. **Panel de Control (Dashboard):** Visualización del resumen métrico del estado global de misiones y recursos.
    *(Insertar Imagen Aquí)*