using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Archivo
{
    public class LstArchivoDTO
    {
        public int Id { get; set; }
        public string NombreArchivo { get; set; }
        public string ContenidoBase64 { get; set; }
    }
}
