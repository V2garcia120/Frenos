using FrenosWeb.Models;

namespace FrenosWeb.Services
{
    public class ServicioService
    {
        public List<Servicio> GetServicios()
        {
            return new List<Servicio>
            {
                new Servicio { Id = 1, Nombre = "Cambio de pastillas", Precio = 2000 },
                new Servicio { Id = 2, Nombre = "Rectificación de discos", Precio = 3500 }
            };
        }
    }
}
