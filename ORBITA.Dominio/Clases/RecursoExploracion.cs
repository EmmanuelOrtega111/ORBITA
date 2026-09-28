using ORBITA.Dominio.Inbterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ORBITA.Dominio.Clases
{
    public enum EstadoRecurso
    {
        Disponible,
        Asignado,
        EnMantenimiento
    }
    public abstract class RecursoExploracion : IAsignable
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Modelo { get; set; }
        public EstadoRecurso Estado { get; set; }
        // Constructor protegido para que solo las clases derivadas puedan instanciarlo
        protected RecursoExploracion(int id, string codigo, string modelo, EstadoRecurso estado)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                throw new ArgumentException("El codigo no puede estar vacio");

            }
            if (string.IsNullOrWhiteSpace(modelo))
            {
                throw new ArgumentException("El modelo no puede estar vacio");
            }

            Id = id;
            Codigo = codigo;
            Modelo = modelo;
            Estado = estado;
        }
        //Sobrecarga de constructor para crear un recurso sin id y con estado disponible
        protected RecursoExploracion(string codigo, string modelo)
            : this (0,codigo,modelo,EstadoRecurso.Disponible)
        {
        }
        public abstract decimal CalcularCOstoOperacion(Decimal parametroUso);
        public virtual bool AsignarMision(int idMision)
        {
            if (Estado == EstadoRecurso.Disponible)
            {
                Estado = EstadoRecurso.Asignado;
                return true;

            }
            return false;
        }
        public virtual bool LiberarRecurso()
        {
            if (Estado == EstadoRecurso.Asignado)
            {
                Estado = EstadoRecurso.Disponible;
                return true;
            }
            return false;
        }
        public virtual bool ConsultarDisponibilidad()
        {
            return Estado == EstadoRecurso.Disponible;
        }
        public override string ToString()
        {
            return $"Id: {Id}, Codigo: {Codigo}, Modelo: {Modelo}, Estado: {Estado}";
        }
    }
}
