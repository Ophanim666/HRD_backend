using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DTO
{
    public class EstadoRequest
    {   
        //para el maneja de los errores // revisar pq cuando se inserta/actualiza un dato correctamente entrega estos datos
        private bool _Ack = true;
        private string _ErrNo = "000000";
        private string _ErrDes = "Proceso realizado correctamente";
        private string _ErrCon = "[N/A]";

        public bool Ack { get { return this._Ack; } set { this._Ack = value; } }
        public string ErrNo { get { return this._ErrNo; } set { this._ErrNo = value; } }
        public string ErrDes { get { return this._ErrDes; } set { this._ErrDes = value; } }
        public string ErrCon { get { return this._ErrCon; } set { this._ErrCon = value; } }

    }
}
