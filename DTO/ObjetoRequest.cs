using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//preguntar pq se llamaria Check.DTO
namespace DTO
{
    public class ObjetoRequest
    {
        public BodyRequest Body {  get; set; }   
        public EstadoRequest Estado { get; set; }
    }
}
