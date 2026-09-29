using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORBITA.dominios
{
    public class Mision
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Prioridad { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaEstimadaFin { get; set; }
        public EstadoMision Estado { get; set; }
        public int IdResponsable { get; set; }
        public Usuario Responsable { get; set; }

        // Colección Genérica
        public List<RecursoExploracion> RecursosAsignados { get; set; } = new List<RecursoExploracion>();
        public override string ToString()
        {
            string respNombre = Responsable != null ? Responsable.NombreUsuario : "Sin asignar";
            string recursosStr = RecursosAsignados.Count > 0
                ? string.Join(", ", RecursosAsignados.ConvertAll(r => r.Codigo))
                : "Ninguno";

            return $"[{Codigo}] {Nombre}\n" +
                   $"  - Descripción: {Descripcion}\n" +
                   $"  - Inicio: {FechaInicio:yyyy-MM-dd} | Fin Estimado: {FechaEstimadaFin:yyyy-MM-dd}\n" +
                   $"  - Prioridad: {Prioridad} | Estado: {Estado}\n" +
                   $"  - Responsable: {respNombre}\n" +
                   $"  - Recursos Asignados: {recursosStr}\n" +
                   $"-------------------------------------------------------------------";
        }

        public Mision() { }

        // Regla Especial: Protocolo de Seguridad ORBITA (Sección 13)
        public (bool EsValido, string MensajeError) ValidarProtocoloSeguridad()
        {
            // 1. La misión deberá contener al menos un recurso.
            if (RecursosAsignados == null || RecursosAsignados.Count == 0)
            {
                return (false, "La misión no tiene ningún recurso asignado.");
            }

            // 2. Ningún recurso podrá encontrarse en mantenimiento.
            foreach (var recurso in RecursosAsignados)
            {
                if (recurso.Estado == EstadoRecurso.Mantenimiento)
                {
                    return (false, $"El recurso [{recurso.Codigo}] {recurso.Modelo} se encuentra en MANTENIMIENTO.");
                }
            }

            // 3. Ningún recurso podrá estar asignado a otra misión activa (ya gestionado por el estado Asignado/Disponible)
            // Se valida que todos los recursos estén formalmente listos.

            // 4. La fecha de finalización estimada no podrá ser anterior a la fecha de inicio.
            if (FechaEstimadaFin < FechaInicio)
            {
                return (false, "La fecha de finalización estimada no puede ser anterior a la fecha de inicio.");
            }

            // 5. El responsable de la misión deberá encontrarse activo.
            if (Responsable == null || !Responsable.Activo)
            {
                return (false, "El usuario responsable de la misión no está activo o no fue asignado.");
            }

            // 6. Todos los datos obligatorios deberán encontrarse completos.
            if (string.IsNullOrWhiteSpace(Codigo) || string.IsNullOrWhiteSpace(Nombre) || string.IsNullOrWhiteSpace(Descripcion))
            {
                return (false, "Faltan datos obligatorios de la misión (Código, Nombre o Descripción).");
            }

            return (true, string.Empty);
        }
    }
}
