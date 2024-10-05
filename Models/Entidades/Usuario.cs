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
        public string Primer_nombre { get; set; }
        public string segundo_nombre { get; set; }
        public string Primer_apellido { get; set; }
        public string Segundo_apellido { get; set; }
        public string Rut { get; set; }
        public string Dv { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Es_administrador { get; set; } //recordar hacerle un hash
        public int Rol_id { get; set; }
        public string Usuario_creacion { get; set; }
        public DateTime Fecha_creacion { get; set; }
    }
}

