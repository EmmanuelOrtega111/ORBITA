using ORBITA.dominios;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORBITA.datos
{
    public class UsuarioRepositorio
    {
        public Usuario Autenticar(string username, string password)
        {
            using (var conexion = ConexionBD.ObtenerConexion())
            {
                conexion.Open();
                string query = "SELECT Id, NombreUsuario, Rol, Activo FROM Usuarios WHERE NombreUsuario = @user AND Contrasena = @pass AND Activo = 1";
                using (var cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@pass", password);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Usuario
                            {
                                Id = reader.GetInt32(0),
                                NombreUsuario = reader.GetString(1),
                                Rol = (RolUsuario)Enum.Parse(typeof(RolUsuario), reader.GetString(2)),
                                Activo = reader.GetBoolean(3)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public List<Usuario> ObtenerTodos()
        {
            var lista = new List<Usuario>();
            using (var conexion = ConexionBD.ObtenerConexion())
            {
                conexion.Open();
                string query = "SELECT Id, NombreUsuario, Rol, Activo FROM Usuarios";
                using (var cmd = new SqlCommand(query, conexion))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Usuario
                        {
                            Id = reader.GetInt32(0),
                            NombreUsuario = reader.GetString(1),
                            Rol = (RolUsuario)Enum.Parse(typeof(RolUsuario), reader.GetString(2)),
                            Activo = reader.GetBoolean(3)
                        });
                    }
                }
            }
            return lista;
        }

        public void RegistrarUsuario(string username, string password, string rol)
        {
            using (var conexion = ConexionBD.ObtenerConexion())
            {
                conexion.Open();
                string query = "INSERT INTO Usuarios (NombreUsuario, Contrasena, Rol, Activo) VALUES (@user, @pass, @rol, 1)";
                using (var cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@pass", password);
                    cmd.Parameters.AddWithValue("@rol", rol);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
