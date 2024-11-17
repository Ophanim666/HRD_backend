using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.GrupoTareas
{
    public class GrupoTareasInsertDTO
    {
        public int ID { get; set; }
        public int ACTA_ID { get; set; }
        public int ROL_ID { get; set; }
        public int ENCARGADO_ID { get; set; }

        public List<int> ListaTareas { get; set; } // IDs de tareas seleccionadas
    }
}
