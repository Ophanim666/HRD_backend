using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Proveedor
{
    public class ProveedorConEspecialidadDTO //TESTING
    {
        //este se utiliza en "Listar proveedores con sus especialidades"
        public int IDproveedor { get; set; }
        public string ProveedorNombre { get; set; }
        public int IDespecialidad { get; set; }
        public string EspecialidadNombre { get; set; }
    }
}
