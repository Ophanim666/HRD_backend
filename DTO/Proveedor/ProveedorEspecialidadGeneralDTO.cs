using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Proveedor
{
    public class ProveedorEspecialidadGeneralDTO
    {
        //este se utiliza en "Listar proveedores con sus especialidades" GENERAL no usa el ID para listar uno en especifico
        public int IDproveedor { get; set; }
        public string ProveedorNombre { get; set; }
        public List<int> IDespecialidades { get; set; } = new List<int>(); // Lista de IDs de especialidades
        public List<string> EspecialidadNombres { get; set; } = new List<string>(); // Lista de nombres de especialidades
    }
}
