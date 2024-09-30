using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Proveedor
{
    public class ProveedorDTO
    {
        public int ID { get; set; }
        public string NOMBRE { get; set; }
        public string RAZON_SOCIAL { get; set; }
        public string RUT { get; set; }
        public string DV { get; set; }
        // se docuemtno 
        //public string NOMBRE_CONTACTO_PRINCIPAL { get; set; }
        //public int NUMERO_CONTACTO_PRINCIPAL { get; set; }
        //public string NOMBRE_CONTACTO_SECUNDARIO { get; set; }
        //public int NUMERO_CONTACTO_SECUNDARIO { get; set; }
        public int ESTADO { get; set; }
        public string USUARIO_CREACION { get; set; }
        public DateTime FECHA_CREACION { get; set; }
    }
}
