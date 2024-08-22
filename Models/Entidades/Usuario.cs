using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entidades
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Rut { get; set; }
        public string Primer_Nombre { get; set; }
        public string Segundo_Nombre { get; set; }
        public string Primer_Apellido { get; set; }
        public string Segundo_Apellido { get; set; }
        public DateTime Fecha_de_nacimiento { get; set; }
        public string Rol { get; set; }
        public string Especialidad { get; set; }
    }
}

