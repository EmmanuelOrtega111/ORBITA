using ORBITA.dominios;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORBITA.datos
{
    public class MisionRepositorio
    {
    
        public List<Mision> ObtenerTodas()
        {
            var misiones = new List<Mision>();

            using (var conexion = ConexionBD.ObtenerConexion())
            {
                conexion.Open();
                string query = @"SELECT m.Id, m.Codigo, m.Nombre, m.Descripcion, m.Prioridad, 
                                        m.FechaInicio, m.FechaEstimadaFin, m.Estado, m.IdResponsable,
                                        u.NombreUsuario, u.Rol, u.Activo
                                 FROM Misiones m
                                 INNER JOIN Usuarios u ON m.IdResponsable = u.Id";

                using (var cmd = new SqlCommand(query, conexion))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var m = new Mision
                        {
                            Id = reader.GetInt32(0),
                            Codigo = reader.GetString(1),
                            Nombre = reader.GetString(2),
                            Descripcion = reader.IsDBNull(3) ? "" : reader.GetString(3),
                            Prioridad = reader.GetInt32(4),
                            FechaInicio = reader.GetDateTime(5),
                            FechaEstimadaFin = reader.GetDateTime(6),
                            Estado = (EstadoMision)Enum.Parse(typeof(EstadoMision), reader.GetString(7)),
                            IdResponsable = reader.GetInt32(8),
                            Responsable = new Usuario
                            {
                                Id = reader.GetInt32(8),
                                NombreUsuario = reader.GetString(9),
                                Rol = (RolUsuario)Enum.Parse(typeof(RolUsuario), reader.GetString(10)),
                                Activo = reader.GetBoolean(11)
                            }
                        };
                        misiones.Add(m);
                    }
                }

                // Cargar los recursos asignados a cada misión
                foreach (var m in misiones)
                {
                    // Se incluye r.Tipo en la consulta para instanciar la clase derivada correspondiente
                    string qRec = @"SELECT r.Id, r.Codigo, r.Modelo, r.Tipo, r.Estado, ISNULL(r.CostoBase, 0) 
                    FROM Asignaciones a 
                    INNER JOIN Recursos r ON a.IdRecurso = r.Id 
                    WHERE a.IdMision = @idMision";

                    using (var cmdRec = new SqlCommand(qRec, conexion))
                    {
                        cmdRec.Parameters.AddWithValue("@idMision", m.Id);
                        using (var readerRec = cmdRec.ExecuteReader())
                        {
                            while (readerRec.Read())
                            {
                                int id = readerRec.GetInt32(0);
                                string codigo = readerRec.GetString(1);
                                string modelo = readerRec.IsDBNull(2) ? "" : readerRec.GetString(2);
                                string tipo = readerRec.IsDBNull(3) ? "Dron" : readerRec.GetString(3);
                                string estadoTexto = readerRec.IsDBNull(4) ? "Disponible" : readerRec.GetString(4);
                                decimal costoBase = Convert.ToDecimal(readerRec[5]);

                                RecursoExploracion recurso = null;

                                // Instanciación polimórfica según el tipo real de la Base de Datos
                                if (tipo == "Dron")
                                {
                                    recurso = new Dron(id, codigo, modelo, costoBase, 0, 0);
                                }
                                else if (tipo == "Rover")
                                {
                                    recurso = new RoverTerrestre(id, codigo, modelo, costoBase, 0, 0);
                                }
                                else if (tipo == "EstacionSensores")
                                {
                                    recurso = new EstacionSensores(id, codigo, modelo, costoBase, 0, 0);
                                }

                                if (recurso != null)
                                {
                                    recurso.Estado = (EstadoRecurso)Enum.Parse(typeof(EstadoRecurso), estadoTexto.Replace(" ", ""));
                                    m.RecursosAsignados.Add(recurso);
                                }
                            }
                        }
                    }
                }
            }
            return misiones;
        }

        public void CrearMision(string codigo, string nombre, string descripcion, int prioridad, DateTime inicio, DateTime fin, int idResponsable)
        {
            using (var conexion = ConexionBD.ObtenerConexion())
            {
                conexion.Open();
                string query = @"INSERT INTO Misiones (Codigo, Nombre, Descripcion, Prioridad, FechaInicio, FechaEstimadaFin, Estado, IdResponsable) 
                                VALUES (@cod, @nom, @desc, @prio, @ini, @fin, 'Planificada', @resp)";

                using (var cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@cod", codigo);
                    cmd.Parameters.AddWithValue("@nom", nombre);
                    cmd.Parameters.AddWithValue("@desc", descripcion);
                    cmd.Parameters.AddWithValue("@prio", prioridad);
                    cmd.Parameters.AddWithValue("@ini", inicio);
                    cmd.Parameters.AddWithValue("@fin", fin);
                    cmd.Parameters.AddWithValue("@resp", idResponsable);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ModificarMision(int id, string nuevoNombre, string nuevaDesc)
        {
            using (var conexion = ConexionBD.ObtenerConexion())
            {
                conexion.Open();
                string query = "UPDATE Misiones SET Nombre = @nom, Descripcion = @desc WHERE Id = @id";
                using (var cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@nom", nuevoNombre);
                    cmd.Parameters.AddWithValue("@desc", nuevaDesc);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void CambiarEstadoMision(int idMision, EstadoMision nuevoEstado)
        {
            using (var conexion = ConexionBD.ObtenerConexion())
            {
                conexion.Open();
                string query = "UPDATE Misiones SET Estado = @est WHERE Id = @id";
                using (var cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@est", nuevoEstado.ToString());
                    cmd.Parameters.AddWithValue("@id", idMision);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AsignarRecursoAMision(int idMision, int idRecurso)
        {
            using (var conexion = ConexionBD.ObtenerConexion())
            {
                conexion.Open();
                string query = "INSERT INTO Asignaciones (IdMision, IdRecurso) VALUES (@idMision, @idRecurso); UPDATE Recursos SET Estado = 'Asignado' WHERE Id = @idRecurso;";
                using (var cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@idMision", idMision);
                    cmd.Parameters.AddWithValue("@idRecurso", idRecurso);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void RetirarRecursoDeMision(int idMision, int idRecurso)
        {
            using (var conexion = ConexionBD.ObtenerConexion())
            {
                conexion.Open();
                string query = @"DELETE FROM Asignaciones WHERE IdMision = @idMision AND IdRecurso = @idRecurso; 
                        UPDATE Recursos SET Estado = 'Disponible' WHERE Id = @idRecurso;";
                using (var cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@idMision", idMision);
                    cmd.Parameters.AddWithValue("@idRecurso", idRecurso);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
