using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Usuario
{
    public class UsuarioTokenDTO
    {
        public string Email { get; set; }
        public int Rol_id { get; set; }
        public bool EsAdministrador { get; set; } 
    }
}
