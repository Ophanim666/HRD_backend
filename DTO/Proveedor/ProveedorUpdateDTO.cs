using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//actualizar este dto despues de que se cambien los procedimientos almacenados
namespace DTO.Proveedor
{
    public class ProveedorUpdateDTO
    {
        //Revisar datos que hay que sacar y cuales se mantienen para actualizar, consultar procedimiento almacenado
        public int ID { get; set; }
        public string NOMBRE { get; set; }
        public string RAZON_SOCIAL { get; set; }
        public string RUT { get; set; }
        public string DV { get; set; }
        public string NOMBRE_CONTACTO_PRINCIPAL { get; set; }
        public int NUMERO_CONTACTO_PRINCIPAL { get; set; }
        public string NOMBRE_CONTACTO_SECUNDARIO { get; set; }
        public int NUMERO_CONTACTO_SECUNDARIO { get; set; }
        public int ESTADO { get; set; }

        //Eliminar si se requiere
        public string USUARIO_CREACION { get; set; }
        public DateTime FECHA_CREACION { get; set; }
    }
}
