using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORBITA.dominios
{ 
    
    public enum RolUsuario
    {
        Administrador,
        Coordinador,
        Auditor,
    }

    public enum EstadoMision
    {
        Planificada,
        EnEjecucion,
        Finalizada,
        Cancelada

    }

    public enum EstadoRecurso
    {
        Disponible,
        Asignado,
        Mantenimiento
    }
    internal class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
