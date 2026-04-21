using System.ComponentModel.DataAnnotations;

public class VehiculoModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "La marca es obligatoria")]
    public string Marca { get; set; } = "";

    [Required(ErrorMessage = "El modelo es obligatorio")]
    public string Modelo { get; set; } = "";

    [Range(1950, 2026, ErrorMessage = "El año debe estar entre 1950 y 2026")]
    public int Anio { get; set; }

    [Required(ErrorMessage = "La placa es obligatoria")]
    [StringLength(7, MinimumLength = 6, ErrorMessage = "Placa inválida")]
    public string Placa { get; set; } = "";
}