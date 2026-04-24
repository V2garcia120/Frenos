using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TallerCaja.Models.Entities
{
    public class CxCLocal
    {
        public int Id { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;
        public string NumeroFactura { get; set; } = string.Empty;
        public decimal Saldo { get; set; }
        public DateTime Vencimiento { get; set; }
        public DateTime UltimaSync { get; set; }
    }
}
