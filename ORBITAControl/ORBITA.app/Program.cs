using ORBITA.datos;
using ORBITA.dominios;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORBITA.app
{
    internal class Program
    {
        private static Usuario usuarioAutenticado = null;
        private static UsuarioRepositorio usuarioRepo = new UsuarioRepositorio();
        private static RecursoRepositorio recursoRepo = new RecursoRepositorio();
        private static MisionRepositorio misionRepo = new MisionRepositorio();

        static void Main(string[] args)
        {
            Console.Title = "SISTEMA ORBITA CONTROL";

            while (true)
            {
                if (usuarioAutenticado == null)
                    MostrarMenuLogin();
                else
                    MostrarMenuPrincipal();
            }
        }

        static void MostrarMenuLogin()
        {
            Console.Clear();
            Console.WriteLine("===========================================");
            Console.WriteLine("    SISTEMA ORBITA CONTROL - INICIO DE SESIÓN");
            Console.WriteLine("===========================================");
            Console.Write("Usuario: ");
            string username = Console.ReadLine();
            Console.Write("Contraseña: ");
            string password = Console.ReadLine();

            usuarioAutenticado = usuarioRepo.Autenticar(username, password);

            if (usuarioAutenticado != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n¡Bienvenido {usuarioAutenticado.NombreUsuario} ({usuarioAutenticado.Rol})!");
                Console.ResetColor();
                Console.WriteLine("Presione cualquier tecla para ingresar...");
                Console.ReadKey();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nCredenciales inválidas. Intente nuevamente.");
                Console.ResetColor();
                Console.WriteLine("Presione cualquier tecla para reintentar...");
                Console.ReadKey();
            }
        }

        static void MostrarMenuPrincipal()
        {
            Console.Clear();
            Console.WriteLine($"===========================================");
            Console.WriteLine($"  ORBITA CONTROL | Usuario: {usuarioAutenticado.NombreUsuario} [{usuarioAutenticado.Rol}]");
            Console.WriteLine($"===========================================");
            Console.WriteLine("1. Gestión de Usuarios (Solo Admin)");
            Console.WriteLine("2. Gestión de Recursos");
            Console.WriteLine("3. Gestión de Misiones");
            Console.WriteLine("4. Panel de Control (Dashboard)");
            Console.WriteLine("5. Probar Protocolo de Seguridad");
            Console.WriteLine("6. Cerrar Sesión");
            Console.WriteLine("0. Salir del Sistema");
            Console.Write("\nSeleccione una opción: ");

            switch (Console.ReadLine())
            {
                case "1": MenuUsuarios(); break;
                case "2": MenuRecursos(); break;
                case "3": MenuMisiones(); break;
                case "4": MostrarDashboard(); break;
                case "5": ProbarProtocoloSeguridad(); break;
                case "6": usuarioAutenticado = null; break;
                case "0": Environment.Exit(0); break;
                default:
                    Console.WriteLine("Opción no válida.");
                    Console.ReadKey();
                    break;
            }
        }

        static void MenuUsuarios()
        {
            if (usuarioAutenticado.Rol != RolUsuario.Administrador)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nAcceso denegado: Solo el rol Administrador puede gestionar usuarios.");
                Console.ResetColor();
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine("=== GESTIÓN DE USUARIOS (ADMINISTRADOR) ===");
            Console.WriteLine("1. Listar Usuarios");
            Console.WriteLine("2. Registrar Nuevo Usuario");
            Console.WriteLine("0. Volver");
            Console.Write("\nOpción: ");

            switch (Console.ReadLine())
            {
                case "1": ListarUsuarios(); break;
                case "2": RegistrarUsuario(); break;
            }
        }

        static void ListarUsuarios()
        {
            Console.Clear();
            Console.WriteLine("=== LISTA DE USUARIOS ===");
            foreach (var u in usuarioRepo.ObtenerTodos())
            {
                Console.WriteLine($"ID: {u.Id} | Usuario: {u.NombreUsuario} | Rol: {u.Rol} | Activo: {u.Activo}");
            }
            Console.ReadKey();
        }

        static void RegistrarUsuario()
        {
            Console.Clear();
            Console.WriteLine("=== REGISTRAR NUEVO USUARIO ===");
            Console.Write("Nombre de usuario: "); string user = Console.ReadLine();
            Console.Write("Contraseña: "); string pass = Console.ReadLine();
            Console.WriteLine("Rol: 1. Administrador | 2. Coordinador | 3. Auditor");
            string rOp = Console.ReadLine();
            string rol = rOp == "1" ? "Administrador" : rOp == "2" ? "Coordinador" : "Auditor";

            usuarioRepo.RegistrarUsuario(user, pass, rol);
            Console.WriteLine("\n¡Usuario creado con éxito en la Base de Datos!");
            Console.ReadKey();
        }

        // ==================== MENÚ DE RECURSOS ====================
        static void MenuRecursos()
        {
            Console.Clear();
            Console.WriteLine("=== GESTIÓN DE RECURSOS ===");
            Console.WriteLine("1. Registrar nuevo recurso");
            Console.WriteLine("2. Modificar datos de un recurso");
            Console.WriteLine("3. Consultar TODOS los recursos");
            Console.WriteLine("4. Consultar ÚNICAMENTE recursos disponibles");
            Console.WriteLine("5. Cambiar el estado de un recurso");
            Console.WriteLine("0. Volver");
            Console.Write("\nOpción: ");

            switch (Console.ReadLine())
            {
                case "1": RegistrarRecurso(); break;
                case "2": ModificarRecurso(); break;
                case "3": ListarRecursos(false); break;
                case "4": ListarRecursos(true); break;
                case "5": CambiarEstadoRecurso(); break;
            }
        }

        static void RegistrarRecurso()
        {
            if (usuarioAutenticado.Rol == RolUsuario.Auditor) { Denegado(); return; }

            Console.Clear();
            Console.WriteLine("=== REGISTRAR RECURSO ===");
            Console.Write("Código (ej. DRN-099): "); string cod = Console.ReadLine();
            Console.Write("Modelo: "); string mod = Console.ReadLine();
            Console.WriteLine("Tipo: 1. Dron | 2. Rover | 3. Estación de Sensores");
            string tOp = Console.ReadLine();

            string tipo = "";
            string etiquetaCosto = "";
            string etiquetaP1 = "";
            string etiquetaP2 = "";

            if (tOp == "1")
            {
                tipo = "Dron";
                etiquetaCosto = "Costo por hora ($USD): ";
                etiquetaP1 = "Autonomía de vuelo (horas): ";
                etiquetaP2 = "Alcance (km): ";
            }
            else if (tOp == "2")
            {
                tipo = "Rover";
                etiquetaCosto = "Costo por kilómetro ($USD): ";
                etiquetaP1 = "Autonomía (km): ";
                etiquetaP2 = "Capacidad de carga (kg): ";
            }
            else
            {
                tipo = "EstacionSensores";
                etiquetaCosto = "Costo diario ($USD): ";
                etiquetaP1 = "Cantidad de sensores: ";
                etiquetaP2 = "Consumo energético (W): ";
            }

            Console.Write(etiquetaCosto); decimal costo = decimal.Parse(Console.ReadLine());
            Console.Write(etiquetaP1); double p1 = double.Parse(Console.ReadLine());
            Console.Write(etiquetaP2); double p2 = double.Parse(Console.ReadLine());

            recursoRepo.RegistrarRecurso(cod, mod, tipo, costo, p1, p2);
            Console.WriteLine("\n¡Recurso registrado exitosamente!");
            Console.ReadKey();
        }

        static void ModificarRecurso()
        {
            if (usuarioAutenticado.Rol == RolUsuario.Auditor) { Denegado(); return; }

            ListarRecursosSinPausa();
            Console.Write("\nID del recurso a modificar: "); int id = int.Parse(Console.ReadLine());
            Console.Write("Nuevo Modelo: "); string mod = Console.ReadLine();
            Console.Write("Nuevo Costo Base: "); decimal costo = decimal.Parse(Console.ReadLine());

            recursoRepo.ModificarModeloYCosto(id, mod, costo);
            Console.WriteLine("\n¡Datos del recurso actualizados!");
            Console.ReadKey();
        }

        static void ListarRecursos(bool soloDisponibles)
        {
            Console.Clear();
            Console.WriteLine(soloDisponibles ? "=== RECURSOS DISPONIBLES ===" : "=== TODOS LOS RECURSOS ===");
            var lista = soloDisponibles ? recursoRepo.ObtenerDisponibles() : recursoRepo.ObtenerTodos();

            foreach (var r in lista)
                Console.WriteLine(r.ToString());

            Console.ReadKey();
        }

        static void CambiarEstadoRecurso()
        {
            if (usuarioAutenticado.Rol == RolUsuario.Auditor) { Denegado(); return; }

            ListarRecursosSinPausa();
            Console.Write("\nID del recurso: "); int id = int.Parse(Console.ReadLine());
            Console.WriteLine("Estado: 1. Disponible | 2. Asignado | 3. Mantenimiento");
            string op = Console.ReadLine();

            EstadoRecurso est = EstadoRecurso.Disponible;
            switch (op)
            {
                case "1": est = EstadoRecurso.Disponible; break;
                case "2": est = EstadoRecurso.Asignado; break;
                case "3": est = EstadoRecurso.Mantenimiento; break;
            }

            recursoRepo.CambiarEstado(id, est);
            Console.WriteLine("\n¡Estado del recurso cambiado!");
            Console.ReadKey();
        }

        // ==================== MENÚ DE MISIONES (MODIFICADO) ====================
        static void MenuMisiones()
        {
            Console.Clear();
            Console.WriteLine("=== GESTION DE MISIONES ===");
            Console.WriteLine("1. Crear una mision");
            Console.WriteLine("2. Modificar una mision");
            Console.WriteLine("3. Consultar misiones registradas");
            Console.WriteLine("4. Asignar recurso a mision");
            Console.WriteLine("5. Retirar recurso de mision");
            Console.WriteLine("6. Iniciar una mision");
            Console.WriteLine("7. Finalizar una mision");
            Console.WriteLine("8. Cancelar una mision");
            Console.WriteLine("9. Calcular costo estimado de operacion");
            Console.WriteLine("0. Volver");
            Console.Write("\nOpcion: ");

            switch (Console.ReadLine())
            {
                case "1": CrearMision(); break;
                case "2": ModificarMision(); break;
                case "3": ListarMisiones(); break;
                case "4": AsignarRecursoMision(); break;
                case "5": RetirarRecursoMision(); break;
                case "6": IniciarMision(); break;
                case "7": CambiarEstadoMision(EstadoMision.Finalizada); break;
                case "8": CambiarEstadoMision(EstadoMision.Cancelada); break;
                case "9": CalcularCostoEstimadoMision(); break;
            }
        }
        static void IniciarMision()
        {
            if (usuarioAutenticado.Rol == RolUsuario.Auditor) { Denegado(); return; }

            ListarMisionesSinPausa();
            Console.Write("\nID de la Mision a Iniciar: ");
            if (!int.TryParse(Console.ReadLine(), out int idM))
            {
                Console.WriteLine("ID no valido.");
                Console.ReadKey();
                return;
            }

            // Se obtiene la misión para ejecutar el Protocolo de Seguridad
            var mision = misionRepo.ObtenerTodas().FirstOrDefault(m => m.Id == idM);

            if (mision == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nNo se encontró la mision especificada.");
                Console.ResetColor();
                Console.ReadKey();
                return;
            }

            // Validación del Protocolo de Seguridad ORBITA (Regla Especial)
            var (esValido, mensajeError) = mision.ValidarProtocoloSeguridad();

            if (!esValido)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[REGLA DE SEGURIDAD RECHAZADA]: {mensajeError}");
                Console.ResetColor();
            }
            else
            {
                misionRepo.CambiarEstadoMision(idM, EstadoMision.EnEjecucion);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n¡La mision [{mision.Codigo}] ha superado el protocolo de seguridad y paso a estado 'EnEjecucion'!");
                Console.ResetColor();
            }

            Console.ReadKey();
        }

        static void CrearMision()
        {
            if (usuarioAutenticado.Rol == RolUsuario.Auditor) { Denegado(); return; }

            Console.Clear();
            Console.WriteLine("=== CREAR MISIÓN ===");
            Console.Write("Código (ej. MIS-099): "); string cod = Console.ReadLine();
            Console.Write("Nombre: "); string nom = Console.ReadLine();
            Console.Write("Descripción: "); string desc = Console.ReadLine();
            Console.Write("Prioridad (1-5): "); int prio = int.Parse(Console.ReadLine());

            misionRepo.CrearMision(cod, nom, desc, prio, DateTime.Now, DateTime.Now.AddDays(10), usuarioAutenticado.Id);
            Console.WriteLine("\n¡Misión creada exitosamente en estado 'Planificada'!");
            Console.ReadKey();
        }

        static void ModificarMision()
        {
            if (usuarioAutenticado.Rol == RolUsuario.Auditor) { Denegado(); return; }

            ListarMisionesSinPausa();
            Console.Write("\nID de la misión a modificar: "); int id = int.Parse(Console.ReadLine());
            Console.Write("Nuevo Nombre: "); string nom = Console.ReadLine();
            Console.Write("Nueva Descripción: "); string desc = Console.ReadLine();

            misionRepo.ModificarMision(id, nom, desc);
            Console.WriteLine("\n¡Misión modificada!");
            Console.ReadKey();
        }

        static void ListarMisiones()
        {
            Console.Clear();
            Console.WriteLine("===================================================================");
            Console.WriteLine("                    === MISIONES REGISTRADAS ===");
            Console.WriteLine("===================================================================");

            var misiones = misionRepo.ObtenerTodas();
            if (misiones.Count == 0)
            {
                Console.WriteLine("No hay misiones registradas.");
            }
            else
            {
                foreach (var m in misiones)
                {
                    Console.WriteLine(m.ToString());
                }
            }

            Console.ReadKey();
        }
        static void CalcularCostoEstimadoMision()
        {
            Console.Clear();
            Console.WriteLine("=== CALCULAR COSTO ESTIMADO DE MISIÓN ===");
            ListarMisionesSinPausa();
            Console.Write("\nIngrese el ID de la Misión: ");

            if (!int.TryParse(Console.ReadLine(), out int idMision))
            {
                Console.WriteLine("ID no válido.");
                Console.ReadKey();
                return;
            }

            var mision = misionRepo.ObtenerTodas().FirstOrDefault(m => m.Id == idMision);
            if (mision == null)
            {
                Console.WriteLine("Misión no encontrada.");
                Console.ReadKey();
                return;
            }

            if (mision.RecursosAsignados.Count == 0)
            {
                Console.WriteLine("\nLa misión no tiene recursos asignados para calcular costos.");
                Console.ReadKey();
                return;
            }

            Console.Write("Ingrese las unidades de uso estimadas (Horas para Drones / Km para Rovers / Días para Estaciones): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal unidades) || unidades < 0)
            {
                Console.WriteLine("Valor numerico no valido.");
                Console.ReadKey();
                return;
            }

            decimal costoTotalMision = 0m;
            Console.WriteLine("\n--- Desglose Polimorfico de Recursos ---");

            foreach (var recurso in mision.RecursosAsignados)
            {
                // Polimorfismo puro: invoca el método heredado sin preguntar el tipo con if/switch
                decimal costoRecurso = recurso.CalcularCostoOperacion(unidades);
                costoTotalMision += costoRecurso;

                Console.WriteLine($"[{recurso.Codigo}] {recurso.Modelo} -> Costo Calculado: ${costoRecurso:F2} USD");
            }

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"COSTO TOTAL ESTIMADO DE LA MISION: ${costoTotalMision:F2} USD");
            Console.ReadKey();
        }
        static void AsignarRecursoMision()
        {
            if (usuarioAutenticado.Rol == RolUsuario.Auditor) { Denegado(); return; }

            ListarMisionesSinPausa();
            Console.Write("\nID Misión: "); int idM = int.Parse(Console.ReadLine());
            ListarRecursosSinPausa();
            Console.Write("ID Recurso: "); int idR = int.Parse(Console.ReadLine());

            misionRepo.AsignarRecursoAMision(idM, idR);
            Console.WriteLine("\n¡Recurso asignado a la misión!");
            Console.ReadKey();
        }

        static void RetirarRecursoMision()
        {
            if (usuarioAutenticado.Rol == RolUsuario.Auditor) { Denegado(); return; }

            ListarMisionesSinPausa();
            Console.Write("\nID Misión: "); int idM = int.Parse(Console.ReadLine());
            Console.Write("ID Recurso a retirar: "); int idR = int.Parse(Console.ReadLine());

            misionRepo.RetirarRecursoDeMision(idM, idR);
            Console.WriteLine("\n¡Recurso retirado y liberado!");
            Console.ReadKey();
        }

        static void CambiarEstadoMision(EstadoMision est)
        {
            if (usuarioAutenticado.Rol == RolUsuario.Auditor) { Denegado(); return; }

            ListarMisionesSinPausa();
            Console.Write("\nID Misión: "); int idM = int.Parse(Console.ReadLine());

            misionRepo.CambiarEstadoMision(idM, est);
            Console.WriteLine($"\n¡Estado de misión actualizado a '{est}'!");
            Console.ReadKey();
        }

        // ==================== AUXILIARES ====================
        static void ListarRecursosSinPausa()
        {
            foreach (var r in recursoRepo.ObtenerTodos())
                Console.WriteLine($"ID: {r.Id} | {r}");
        }

        static void ListarMisionesSinPausa()
        {
            foreach (var m in misionRepo.ObtenerTodas())
                Console.WriteLine($"ID: {m.Id} | [{m.Codigo}] {m.Nombre} | Estado: {m.Estado}");
        }

        static void MostrarDashboard()
        {
            Console.Clear();
            Console.WriteLine("=== PANEL DE CONTROL ORBITA (DASHBOARD) ===");
            var misiones = misionRepo.ObtenerTodas();
            var recursos = recursoRepo.ObtenerTodos();

            Console.WriteLine($"\n--- MISIONES ---");
            Console.WriteLine($"Total Misiones: {misiones.Count}");
            Console.WriteLine($"  - Planificadas: {misiones.Count(m => m.Estado == EstadoMision.Planificada)}");
            Console.WriteLine($"  - En Ejecución: {misiones.Count(m => m.Estado == EstadoMision.EnEjecucion)}");
            Console.WriteLine($"  - Finalizadas:   {misiones.Count(m => m.Estado == EstadoMision.Finalizada)}");

            Console.WriteLine($"\n--- RECURSOS ---");
            Console.WriteLine($"Total Recursos: {recursos.Count}");
            Console.WriteLine($"  - Disponibles:   {recursos.Count(r => r.Estado == EstadoRecurso.Disponible)}");
            Console.WriteLine($"  - Asignados:     {recursos.Count(r => r.Estado == EstadoRecurso.Asignado)}");
            Console.WriteLine($"  - Mantenimiento: {recursos.Count(r => r.Estado == EstadoRecurso.Mantenimiento)}");

            Console.ReadKey();
        }

        static void ProbarProtocoloSeguridad()
        {
            Console.Clear();
            Console.WriteLine("=== PRUEBA PROTOCOLO DE SEGURIDAD ===");
            try
            {
                var m = new Mision { Codigo = "TEST", Nombre = "Prueba", Responsable = usuarioAutenticado };
                m.RecursosAsignados.Add(new Dron(3, "DRN-003", "Scout Pro", 90m, 30, 10) { Estado = EstadoRecurso.Mantenimiento });

                var (val, err) = m.ValidarProtocoloSeguridad();
                if (!val) throw new RecursoNoDisponibleException(err);
            }
            catch (RecursoNoDisponibleException ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Excepción capturada: {ex.Message}");
                Console.ResetColor();
            }
            Console.ReadKey();
        }

        static void Denegado()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nAcceso denegado: El rol Auditor solo tiene permisos de lectura.");
            Console.ResetColor();
            Console.ReadKey();
        }
    }
}
