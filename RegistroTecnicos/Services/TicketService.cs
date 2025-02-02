using RegistroTecnicos.Models;
using RegistroTecnicos.DAL;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace RegistroTecnicos.Services
{
    public class TicketService(IDbContextFactory<Contexto> DbFactory)
    {
        public async Task<bool> Guardar(Tickets tickets)
        {
            if (!await Existe(tickets.Prioridad, tickets.TicketId))
            {
                return await Insertar(tickets);
            }
            else
            {
                return await Modificar(tickets);
            }
        }

        public async Task<bool> Existe(string tickets, int ticketId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Tickets
                .AnyAsync(t => t.Prioridad.ToLower() == tickets.ToLower() && t.TicketId != ticketId);
        }

        private async Task<bool> Insertar(Tickets tickets)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            contexto.Tickets.Add(tickets);
            return await contexto
                .SaveChangesAsync() > 0;
        }

        private async Task<bool> Modificar(Tickets tickets)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            contexto.Update(tickets);
            return await contexto
                .SaveChangesAsync() > 0;
        }

        public async Task<Tickets?> Buscar(int ticketId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Tickets.Include(d => d.TicketId)
                .FirstOrDefaultAsync(p => p.TicketId == ticketId);
        }

        public async Task<bool> Eliminar(int ticketId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Tickets.AsNoTracking()
                .Where(p => p.TicketId == ticketId)
                .ExecuteDeleteAsync() > 0;
        }

        public async Task<List<Tickets>> Listar(Expression<Func<Tickets, bool>> criterio)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Tickets
                .Where(criterio)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
