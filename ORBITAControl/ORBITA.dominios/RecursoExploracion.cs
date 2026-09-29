using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORBITA.dominios
{
    public abstract class RecursoExploracion : IAsignable
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Modelo { get; set; }
        public EstadoRecurso Estado { get; set; }
        public decimal CostoBase { get; set; }

        // Constructores (Sobrecarga)
        protected RecursoExploracion() { }

        protected RecursoExploracion(int id, string codigo, string modelo, decimal costoBase)
        {
            Id = id;
            Codigo = codigo;
            Modelo = modelo;
            Estado = EstadoRecurso.Disponible;
            CostoBase = costoBase;
        }

        // Método Polimórfico Abstracto
        public abstract decimal CalcularCostoOperacion(decimal parametro);

        // Métodos de IAsignable
        public bool ConsultarDisponibilidad()
        {
            return Estado == EstadoRecurso.Disponible;
        }

        public void AsignarAMision(int idMision)
        {
            if (!ConsultarDisponibilidad())
            {
                throw new RecursoNoDisponibleException($"El recurso {Codigo} no está disponible (Estado: {Estado}).");
            }
            Estado = EstadoRecurso.Asignado;
        }

        public void LiberarRecurso()
        {
            Estado = EstadoRecurso.Disponible;
        }

        // Sobrescritura de ToString()
        public override string ToString()
        {
            return $"[{Codigo}] {Modelo} | Estado: {Estado}";
        }
    }
}
