using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TallerCaja.Models.Entities
{
    public class ClienteLocal
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Cedula { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public int EsAnonimo { get; set; }
        public DateTime UltimaSync { get; set; }
    }
}
