using System;
using System.Collections.Generic;
using System.Text;

namespace ORBITA.Dominio.Inbterfaces
{
    public interface IAsignable
    {
        bool AsignarMision(int idMision);
        bool LiberarRecurso();
        bool ConsultarDisponibilidad();
    }
}
