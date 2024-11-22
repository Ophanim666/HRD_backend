using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entidades
{
    public class Archivo
    {
        public int Id { get; set; }
        public int Grupo_Tarea_Id { get; set; }
        public string Nombre_Archivo { get; set; }
        public string Ruta_Archivo { get; set; }
        public string Tipo_Imagen { get; set; }
        public DateTime Fecha_Creacion { get; set; }
    }
}
