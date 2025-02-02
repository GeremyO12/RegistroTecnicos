using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace RegistroTecnicos;

public class Clientes
{
    [Key]
    public int ClienteId { get; set; }
    public DateTime FechaIngreso { get; set; }
    public string Nombres { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Rnc { get; set; } = string.Empty;
    [Precision(18, 2)]
    public decimal LimiteCredito { get; set; }
    public int TecnicoId { get; set; }
}
