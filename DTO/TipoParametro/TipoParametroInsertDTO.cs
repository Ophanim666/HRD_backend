using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.TipoParametro
{
    public class TipoParametroInsertDTO
    {
        public int ID { get; set; }
        public string TIPO_PARAMETRO { get; set; }
        public int ESTADO { get; set; }
        //se deshabilito en la bd
        //public string USUARIO_CREACION { get; set; }
        //public DateTime FECHA_CREACION { get; set; }
    }
}
