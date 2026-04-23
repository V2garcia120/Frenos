using TallerCaja.Helpers;
using TallerCaja.Models.DTOs;

namespace TallerCaja.Services
{
    public class ReciboService : IReciboService
    {
        public string GenerarTextoRecibo(CobroResponse cobro, CobroRequest request, string clienteNombre)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("═══════════════════════════════════════");
            sb.AppendLine($"       {AppConfig.NombreTaller}");
            sb.AppendLine("═══════════════════════════════════════");
            sb.AppendLine($" Factura: {cobro.NumeroFactura ?? cobro.IdLocal[..8].ToUpper()}");
            sb.AppendLine($" Fecha:   {DateTime.Now:dd/MM/yyyy HH:mm}");
            sb.AppendLine($" Cajero:  {SessionManager.CajeroNombre}");
            sb.AppendLine($" Cliente: {clienteNombre}");
            sb.AppendLine("───────────────────────────────────────");
            sb.AppendLine($" {"Descripción",-22} {"Cant",4} {"Precio",10}");
            sb.AppendLine("───────────────────────────────────────");

            foreach (var item in request.Items)
            {
                var nombre = item.NombreSnapshot.Length > 22
                    ? item.NombreSnapshot[..22]
                    : item.NombreSnapshot;
                var precio = (item.PrecioSnapshot * item.Cantidad).ToString("N2");
                sb.AppendLine($" {nombre,-22} {item.Cantidad,4} {precio,10}");
            }

            var (subtotal, itbis, total) = FacturaCalculator.Calcular(request.Items);
            sb.AppendLine("───────────────────────────────────────");
            sb.AppendLine($" {"Subtotal:",-22}      {subtotal,10:N2}");
            sb.AppendLine($" {"ITBIS (18%):",-22}      {itbis,10:N2}");
            sb.AppendLine($" {"TOTAL:",-22}      {total,10:N2}");
            sb.AppendLine("───────────────────────────────────────");
            sb.AppendLine($" Método de pago: {request.MetodoPago}");
            sb.AppendLine($" Monto pagado:   RD$ {request.MontoPagado:N2}");
            if (request.MetodoPago == "Efectivo")
                sb.AppendLine($" Cambio:         RD$ {cobro.Cambio:N2}");
            sb.AppendLine($" Estado:         {cobro.Estado}");
            sb.AppendLine("═══════════════════════════════════════");
            sb.AppendLine("    ¡Gracias por su preferencia!");
            sb.AppendLine("═══════════════════════════════════════");
            return sb.ToString();
        }
    }
}
