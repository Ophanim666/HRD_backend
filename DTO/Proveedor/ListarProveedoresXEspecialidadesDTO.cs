using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Proveedor
{
    public class ListarProveedoresXEspecialidadesDTO
    {
        public int IDproveedor { get; set; }
        public string NombreProveedor { get; set; }
        public string RazonSocial { get; set; }
        public string Rut { get; set; }
        public string Dv { get; set; }
        public string NombreContactoPri { get; set; }
        public int NumeroContactoPri { get; set; }
        public string NombreContactoSec { get; set; }
        public int NumeroContactoSec { get; set; }
        public int Estado { get; set; }

        public List<int> IDespecialidad { get; set; } = new List<int>();
        public List<string> NombreEspecialidad { get; set; } = new List<string>();

    }

}

