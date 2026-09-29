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
1. Descargue el archivo con la solucion y los datos del programa (puede hacerlo desde el siguiente link tambien " https://github.com/EmmanuelOrtega111/ORBITA ")
2. Abra el archivo de solución `ORBITA.sln` en Visual Studio.
3. Establezca el proyecto **`ORBITA.app`** como **Proyecto de Inicio** (*Set as Startup Project*).
4. Compile la solución (`Ctrl + Shift + B`).
5. Inicie la aplicación (`F5` o `Ctrl + F5`).

---

## 🔑 3. Credenciales de Prueba

| Usuario | Contraseña | Rol | Permisos Principales |
| :--- | :--- | :--- | :--- |
| `admin_user` | `admin123` | **Administrador** | Acceso total: Gestión de usuarios, recursos y misiones. |
| `coord_user` | `coord123` | **Coordinador** | Gestión operativa de recursos, misiones y asignaciones. |
| `audit_user` | `audit123` | **Auditor** | Solo lectura: Consultas e informes (operaciones bloqueadas). |

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
   <img width="384" height="157" alt="Captura de pantalla 2026-09-28 222808" src="https://github.com/user-attachments/assets/5ce58514-76d4-4d5a-abac-4b14da17bf43" />
2. **Rechazo de credenciales:** Intento de inicio de sesión con usuario o contraseña errónea.
   <img width="370" height="154" alt="Captura de pantalla 2026-09-28 223007" src="https://github.com/user-attachments/assets/2dab37ea-7585-4d80-946f-f945d7116325" />
3. **Bloqueo por Rol Auditor:** Intento de creación o modificación de recursos/misiones desde la cuenta de un usuario con rol Auditor, mostrando mensaje de acceso denegado.
   <img width="522" height="255" alt="Captura de pantalla 2026-09-28 223147" src="https://github.com/user-attachments/assets/f7e50f1c-6a32-4642-a6fe-2306df3837e3" />

---

### Pruebas del Módulo de Recursos
4. **Registro de Recurso (Dron/Rover/Estación):** Alta exitosa de un recurso asignándole su tipo y parámetros específicos.
   <img width="418" height="169" alt="Captura de pantalla 2026-09-28 223739" src="https://github.com/user-attachments/assets/03ef296f-bac5-4206-a461-663af58713e2" />
5. **Consulta de Recursos Disponibles:** Listado filtrado mostrando únicamente los recursos con estado `Disponible`.
   <img width="922" height="147" alt="Captura de pantalla 2026-09-28 223812" src="https://github.com/user-attachments/assets/823bfd46-fdb1-421b-a5e9-0c6e4b8258db" />
6. **Cambio de Estado de Recurso:** Modificación del estado del recurso a `Mantenimiento` o `Asignado`.
   <img width="937" height="305" alt="Captura de pantalla 2026-09-28 223907" src="https://github.com/user-attachments/assets/b6c5700f-f429-4793-8c3c-49b686744011" />

---

### Pruebas del Módulo de Misiones y Asignación
7. **Creación de Misión:** Registro de una nueva misión en estado `Planificada`.
   <img width="428" height="133" alt="Captura de pantalla 2026-09-28 224131" src="https://github.com/user-attachments/assets/6cd6a130-8f66-46c2-abd0-ee8ef675ce03" />
8. **Asignación de Recursos a Misión:** Enlace exitoso entre un recurso disponible y una misión activa.
   <img width="955" height="385" alt="Captura de pantalla 2026-09-28 224244" src="https://github.com/user-attachments/assets/a3165210-f4b7-41cb-9f41-48f2a62bd5d3" />
9. **Cálculo de Costo Estimado:** Invocación del método `CalcularCostoOperacion()` según el tipo específico de recurso (Horas en Drones, Kilómetros en Rovers, Días en Estaciones).
   <img width="824" height="260" alt="Captura de pantalla 2026-09-28 224350" src="https://github.com/user-attachments/assets/a95b8adc-8b45-4a53-a951-a6ad16aea03f" />
