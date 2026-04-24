using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TallerCaja.Models.DTOs
{
    public record RegistrarPagoRequest(
        [Required] string MetodoPago,
        [Required] decimal MontoPagado
    );
}
