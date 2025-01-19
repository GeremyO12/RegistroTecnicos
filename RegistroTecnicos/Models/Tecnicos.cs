using System.ComponentModel.DataAnnotations;

namespace RegistroTecnicos.Models;

public class Tecnicos
{
    [Key]
    [Range(1, int.MaxValue, ErrorMessage = "El ID debe ser mayor o igual que 1.")]
    public int TecnicoId { get; set; }
    [Required(ErrorMessage = "Este campo es requerido")]
    public string? Nombre { get; set; }
    [Range (0.01, double.MaxValue, ErrorMessage = "El campo sueldo por hora debe ser mayor que cero.")]
    public int SueldoHora { get; set; }
}       
