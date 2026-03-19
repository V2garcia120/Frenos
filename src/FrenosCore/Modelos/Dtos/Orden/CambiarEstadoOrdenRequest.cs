using System.ComponentModel.DataAnnotations;

namespace FrenosCore.Modelos.Dtos.Orden
{
    public record CambiarEstadoOrdenRequest(
    [Required] string Estado
    );
}
