using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Acta
{
    public class EstadoActaDTO
    {
        public int? IdEstado { get; set; }  // 1 = Firmado, 0 = Rechazado, null = Sin firmar
    }
}
