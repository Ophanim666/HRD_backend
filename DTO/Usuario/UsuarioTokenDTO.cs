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
        public int EsAdministrador { get; set; }
        // este id llega nulo si no funciona comentar per es necesario para insertar el usuario en acta
        public int Id { get; set; }
        public string Primer_nombre { get; set; }
    }
}
