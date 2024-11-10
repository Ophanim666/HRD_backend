using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entidades
{
    public class Obra
    {
        public int ID { get; set; }
        public string NOMBRE { get; set; }
        public int ESTADO { get; set; }
        public string USUARIO_CREACION { get; set; }
        public DateTime FECHA_CREACION { get; set; }
    }
}
