using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORBITA.dominios
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Contrasena { get; set; }
        public RolUsuario Rol { get; set; }
        public bool Activo { get; set; }

        public Usuario() { }

        public Usuario(int id, string nombreUsuario, string contrasena, RolUsuario rol, bool activo = true)
        {
            Id = id;
            NombreUsuario = nombreUsuario;
            Contrasena = contrasena;
            Rol = rol;
            Activo = activo;
        }
    }
}
