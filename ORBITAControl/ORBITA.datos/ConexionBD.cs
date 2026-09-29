using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORBITA.datos
{
    public static class ConexionBD
    {
        // Cadena de conexión con 'TrustServerCertificate=True' para evitar el error de SSL
        private static string cadenaConexion = @"Server=localhost;Database=ORBITAControlDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public static SqlConnection ObtenerConexion()
        {
            return new SqlConnection(cadenaConexion);
        }
    }
}
