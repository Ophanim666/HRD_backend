using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//actualizar este dto desoues de que se cambien los procedimientos almacenados
namespace DTO.Proveedor
{
    public class ProveedorInsertDTO
    {
        public int ID { get; set; }
        public string NOMBRE { get; set; }
        public string RAZON_SOCIAL { get; set; }
        public string RUT { get; set; }
        public string DV { get; set; }
        //eliminar cuando sea necesario
        //public string NOMBRE_CONTACTO_PRINCIPAL { get; set; }
        //public int NUMERO_CONTACTO_PRINCIPAL { get; set; }
        //public string NOMBRE_CONTACTO_SECUNDARIO { get; set; }
        //public int NUMERO_CONTACTO_SECUNDARIO { get; set; }
        public int ESTADO { get; set; }

        //
        public List<int> ListaEspecialidades { get; set; } // IDs de especialidades seleccionadas
    }
}
