using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.GrupoTareas
{
    public class ListarTareasXGrupoTareasDTO
    {
        //Estos datos son los que se pieden de el procedimiento  "[usp_ListarTareasxGrupoTareas]"
        public int IDGrupoTarea { get; set; }
        public int IDActa { get; set; }
        public int IDRol { get; set; }
        public int IDEncargado { get; set; }
        public string UsuarioCreacion { get; set; }

        public DateTime FechaCreacion { get; set; }

        public List<int> IDTarea { get; set; } = new List<int>();
    }
}
