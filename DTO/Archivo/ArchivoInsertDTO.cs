using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Archivo
{
    public class ArchivoInsertDTO
    {
        public int Grupo_Tarea_Id { get; set; }
        public string Nombre_Archivo { get; set; }
        public string Ruta_Archivo { get; set; }
        public string Tipo_Imagen { get; set; }

    }
}
