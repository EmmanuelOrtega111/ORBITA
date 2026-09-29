using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORBITA.dominios
{
    public class RoverTerrestre : RecursoExploracion
    {
        public double Autonomia { get; set; }
        public double CapacidadCarga { get; set; }

        public RoverTerrestre() { }

        public RoverTerrestre(int id, string codigo, string modelo, decimal costoPorKm, double autonomia, double capacidadCarga)
            : base(id, codigo, modelo, costoPorKm)
        {
            Autonomia = autonomia;
            CapacidadCarga = capacidadCarga;
        }

        // Polimorfismo: Calcula costo según kilómetros
        public override decimal CalcularCostoOperacion(decimal kilometros)
        {
            return CostoBase * kilometros;
        }

        public override string ToString()
        {
            return $"[ROVER] [{Codigo}] {Modelo} | Estado: {Estado} | Autonomía: {Autonomia}km | Carga: {CapacidadCarga}kg | Costo/Km: ${CostoBase} USD";
        }
    }
}
