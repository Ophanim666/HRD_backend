using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Proveedor
{
    public class BuscarProveedorConEspecialidadDTO
    {
        public int IDproveedor { get; set; }
        public int IDespecialidad { get; set; }
        public string EspecialidadNombre { get; set; }
    }
}
