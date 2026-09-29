using ORBITA.dominios;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORBITA.datos
{
    public class RecursoRepositorio
    {
        public List<RecursoExploracion> ObtenerTodos()
        {
            return ObtenerPorConsulta("SELECT Id, Codigo, Modelo, Tipo, Estado, AutonomiaVuelo, Alcance, Autonomia, CapacidadCarga, CantidadSensores, ConsumoEnergetico, CostoBase FROM Recursos");
        }

        public List<RecursoExploracion> ObtenerDisponibles()
        {
            return ObtenerPorConsulta("SELECT Id, Codigo, Modelo, Tipo, Estado, AutonomiaVuelo, Alcance, Autonomia, CapacidadCarga, CantidadSensores, ConsumoEnergetico, CostoBase FROM Recursos WHERE Estado = 'Disponible'");
        }

        private List<RecursoExploracion> ObtenerPorConsulta(string query)
        {
            var lista = new List<RecursoExploracion>();

            using (var conexion = ConexionBD.ObtenerConexion())
            {
                conexion.Open();
                using (var cmd = new SqlCommand(query, conexion))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string tipo = reader.GetString(3);
                        RecursoExploracion recurso = null;

                        int id = reader.GetInt32(0);
                        string codigo = reader.GetString(1);
                        string modelo = reader.GetString(2);
                        EstadoRecurso estado = (EstadoRecurso)Enum.Parse(typeof(EstadoRecurso), reader.GetString(4));
                        decimal costoBase = reader.GetDecimal(11);

                        if (tipo == "Dron")
                        {
                            recurso = new Dron(id, codigo, modelo, costoBase,
                                reader.IsDBNull(5) ? 0 : reader.GetDouble(5),
                                reader.IsDBNull(6) ? 0 : reader.GetDouble(6));
                        }
                        else if (tipo == "Rover")
                        {
                            recurso = new RoverTerrestre(id, codigo, modelo, costoBase,
                                reader.IsDBNull(7) ? 0 : reader.GetDouble(7),
                                reader.IsDBNull(8) ? 0 : reader.GetDouble(8));
                        }
                        else if (tipo == "EstacionSensores")
                        {
                            recurso = new EstacionSensores(id, codigo, modelo, costoBase,
                                reader.IsDBNull(9) ? 0 : reader.GetInt32(9),
                                reader.IsDBNull(10) ? 0 : reader.GetDouble(10));
                        }

                        if (recurso != null)
                        {
                            recurso.Estado = estado;
                            lista.Add(recurso);
                        }
                    }
                }
            }
            return lista;
        }

        public void RegistrarRecurso(string codigo, string modelo, string tipo, decimal costoBase, double p1, double p2)
        {
            using (var conexion = ConexionBD.ObtenerConexion())
            {
                conexion.Open();
                string query = "";

                if (tipo == "Dron")
                    query = "INSERT INTO Recursos (Codigo, Modelo, Tipo, Estado, AutonomiaVuelo, Alcance, CostoBase) VALUES (@cod, @mod, @tipo, 'Disponible', @p1, @p2, @costo)";
                else if (tipo == "Rover")
                    query = "INSERT INTO Recursos (Codigo, Modelo, Tipo, Estado, Autonomia, CapacidadCarga, CostoBase) VALUES (@cod, @mod, @tipo, 'Disponible', @p1, @p2, @costo)";
                else
                    query = "INSERT INTO Recursos (Codigo, Modelo, Tipo, Estado, CantidadSensores, ConsumoEnergetico, CostoBase) VALUES (@cod, @mod, @tipo, 'Disponible', @p1, @p2, @costo)";

                using (var cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@cod", codigo);
                    cmd.Parameters.AddWithValue("@mod", modelo);
                    cmd.Parameters.AddWithValue("@tipo", tipo);
                    cmd.Parameters.AddWithValue("@costo", costoBase);
                    cmd.Parameters.AddWithValue("@p1", p1);
                    cmd.Parameters.AddWithValue("@p2", p2);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ModificarModeloYCosto(int id, string nuevoModelo, decimal nuevoCosto)
        {
            using (var conexion = ConexionBD.ObtenerConexion())
            {
                conexion.Open();
                string query = "UPDATE Recursos SET Modelo = @mod, CostoBase = @costo WHERE Id = @id";
                using (var cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@mod", nuevoModelo);
                    cmd.Parameters.AddWithValue("@costo", nuevoCosto);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void CambiarEstado(int idRecurso, EstadoRecurso nuevoEstado)
        {
            using (var conexion = ConexionBD.ObtenerConexion())
            {
                conexion.Open();
                string query = "UPDATE Recursos SET Estado = @estado WHERE Id = @id";
                using (var cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@estado", nuevoEstado.ToString());
                    cmd.Parameters.AddWithValue("@id", idRecurso);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
