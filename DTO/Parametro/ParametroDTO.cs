using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Parametro
{
    public class ParametroDTO
    {
        public int ID { get; set; }
        public string PARAMETRO { get; set; }
        public string VALOR { get; set; }
        public int ID_TIPO_PARAMETRO { get; set; }
        public int ESTADO { get; set; }
        public string USUARIO_CREACION { get; set; }
        public DateTime FECHA_CREACION { get; set; }
    }
}
