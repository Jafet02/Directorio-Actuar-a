using System;
using System.Collections.Generic;
using System.Text;

namespace DirecAct.Models
{
    public class Funcionarios
    {
        public string RFC { get; set; }
        public string Nombre { get; set; }
        public string Cargo { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Foto { get; set; }
        public int ID_Padre { get; set; }
        public int ID_Adscripcion { get; set; }
        public string Celular { get; set; }
        public string Pass { get; set; }

        public string Padre { get; set; }
        public string Adscripcion { get; set; }
    }
}
