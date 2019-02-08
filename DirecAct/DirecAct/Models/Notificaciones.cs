using System;

namespace DirecAct.Models
{
    public class Notificaciones
    {
        public int ID { get; set; }
        public string Texto { get; set; }
        public string Remitente { get; set; }
        public string Destino { get; set; }
        public DateTime Fecha { get; set; }
    }
}
