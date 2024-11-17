using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entidades
{
    public class GrupoTareas
    {
        public int ID { get; set; }
        public int ACTA_ID { get; set; }
        public int ROL_ID { get; set; }
        public int ENCARGADO_ID { get; set; }
        public string USUARIO_CREACION { get; set; }
        public DateTime FECHA_CREACION { get; set; }
    }
}
