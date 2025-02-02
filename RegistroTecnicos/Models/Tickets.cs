using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace RegistroTecnicos.Models;

public class Tickets
{
    [Key]
    public int TicketId { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
    public string Prioridad { get; set; } = string.Empty;
    public int ClienteId { get; set; }
    public string Asunto { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    [Precision(18, 2)]
    public decimal TiempoInvertido { get; set; }
    public int TecnicoId { get; set; }
}
