using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TallerCaja.Models.Entities
{
    public class FacturasPendienteLocal
    {
        public int Id { get; set; }
        public string Numero { get; set; } = string.Empty;
        public string ClienteNombre { get; set; } = string.Empty;
        public string VehiculoPlaca { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public bool Pagada { get; set; }
        public DateTime UltimaSync { get; set; }
    }
}
