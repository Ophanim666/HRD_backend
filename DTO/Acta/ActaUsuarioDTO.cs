using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Acta
{
    public class ActaUsuarioDTO
    {
        public int Grupo { get; set; }
        public int Acta { get; set; }
        public int Rol { get; set; }
        public int Encargado { get; set; }
        public int Tarea { get; set; }
    }
}
