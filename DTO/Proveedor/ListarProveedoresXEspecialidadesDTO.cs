using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Proveedor
{
    public class ListarProveedoresXEspecialidadesDTO
    {
        //Estos datos son los que se pieden de el procedimiento  "usp_ListarProveedoresConEspecialidadesTESTING"
        public int IDproveedor { get; set; }
        public string NombreProveedor { get; set; }
        public string RazonSocial { get; set; }
        public string Rut { get; set; }
        public string Dv { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int Estado { get; set; }
        public string UsuarioCreacion { get; set; } 

        public List<int> IDespecialidad { get; set; } = new List<int>();
        public List<string> NombreEspecialidad { get; set; } = new List<string>();

    }

}

