using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Acta
{
    public class ActaUpdateDTO
    {
        public int ID { get; set; }
        public int OBRA_ID { get; set; }
        public int PROVEEDOR_ID { get; set; }
        public int ESPECIALIDAD_ID { get; set; }
        public int ESTADO_ID { get; set; }
        public DateTime FECHA_APROBACION { get; set; }
        public string OBSERVACION { get; set; }
        public int REVISOR_ID { get; set; }
    }
}
