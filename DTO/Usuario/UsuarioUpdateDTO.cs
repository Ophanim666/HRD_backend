using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Usuario
{
    public class UsuarioUpdateDTO
    {
        public string Primer_nombre { get; set; }
        public string Segundo_nombre { get; set; }
        public string Primer_apellido { get; set; }
        public string Segundo_apellido { get; set; }
        public string Rut { get; set; }
        public string Dv { get; set; }
        public string Email { get; set; }
        public bool Es_administrador { get; set; }
        public int Rol_id { get; set; }
        public int Estado { get; set; }
    }
}
