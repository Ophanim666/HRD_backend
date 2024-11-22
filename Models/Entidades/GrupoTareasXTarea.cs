using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entidades
{
    public class GrupoTareasXTarea
    {
        public int grupo_tarea_id { get; set; } 
        public int tarea_id { get; set; }
        public int estado { get; set; }
    }
}
