using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Usuario
{
    public class UsuarioInsertDTO
    {
        public string Primer_nombre { get; set; }
        public string Segundo_nombre { get; set; }
        public string Primer_apellido { get; set; }
        public string Segundo_apellido { get; set; }
        public string Rut { get; set; }
        public string Dv { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } //debe de se hashed
        public int Es_administrador { get; set; }
        public int Rol_id { get; set; }
        public int Estado { get; set; }
    }
}
