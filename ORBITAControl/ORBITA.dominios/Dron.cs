using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORBITA.dominios
{
    public class Dron : RecursoExploracion
    {
        public double AutonomiaVuelo { get; set; }
        public double Alcance { get; set; }

        public Dron() { }

        public Dron(int id, string codigo, string modelo, decimal costoPorHora, double autonomia, double alcance)
            : base(id, codigo, modelo, costoPorHora)
        {
            AutonomiaVuelo = autonomia;
            Alcance = alcance;
        }

        // Polimorfismo: Calcula costo según horas
        public override decimal CalcularCostoOperacion(decimal horas)
        {
            return CostoBase * horas;
        }

        public override string ToString()
        {
            return $"[DRON] [{Codigo}] {Modelo} | Estado: {Estado} | Autonomía: {AutonomiaVuelo}h | Alcance: {Alcance}km | Costo/Hora: ${CostoBase} USD";
        }
    }
}
