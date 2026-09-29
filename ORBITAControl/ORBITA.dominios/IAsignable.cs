using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORBITA.dominios
{
    public interface IAsignable
    {
        bool ConsultarDisponibilidad();
        void AsignarAMision(int idMision);
        void LiberarRecurso();
    }
}
