using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORBITA.dominios
{
    public class EstacionSensores : RecursoExploracion
    {
        public int CantidadSensores { get; set; }
        public double ConsumoEnergetico { get; set; }

        public EstacionSensores() { }

        public EstacionSensores(int id, string codigo, string modelo, decimal costoDiario, int cantidadSensores, double consumo)
            : base(id, codigo, modelo, costoDiario)
        {
            CantidadSensores = cantidadSensores;
            ConsumoEnergetico = consumo;
        }

        // Polimorfismo: Calcula costo según días
        public override decimal CalcularCostoOperacion(decimal dias)
        {
            return CostoBase * dias;
        }

        public override string ToString()
        {
            return $"[ESTACIÓN] [{Codigo}] {Modelo} | Estado: {Estado} | Sensores: {CantidadSensores} | Consumo: {ConsumoEnergetico}W | Costo/Día: ${CostoBase} USD";
        }
    }
}
