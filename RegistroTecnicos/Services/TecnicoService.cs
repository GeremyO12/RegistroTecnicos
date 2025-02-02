using RegistroTecnicos.Models;
using RegistroTecnicos.DAL;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace RegistroTecnicos.Services;

public class TecnicoService(IDbContextFactory<Contexto> DbFactory)
{
    public async Task<bool> Guardar(Tecnicos tecnicos)
    {
        if (!await Existe(tecnicos.Nombre, tecnicos.TecnicoId))
        {
            return await Insertar(tecnicos);
        }
        else
        {
            return await Modificar(tecnicos);
        }
    }

    public async Task<bool> Existe(string nombre, int tecnicoId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Tecnicos
            .AnyAsync(t => t.Nombre.ToLower() == nombre.ToLower() && t.TecnicoId != tecnicoId);
    }

    private async Task<bool> Insertar(Tecnicos tecnico)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Tecnicos.Add(tecnico);
        return await contexto
            .SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Tecnicos tecnico)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Update(tecnico);
        return await contexto
            .SaveChangesAsync() > 0;
    }

    public async Task<Tecnicos?> Buscar(int tecnicoId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Tecnicos.Include(d => d.TecnicoId)
            .FirstOrDefaultAsync(p => p.TecnicoId == tecnicoId);
    }

    public async Task<bool> Eliminar(int tecnicoId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Tecnicos.AsNoTracking()
            .Where(p => p.TecnicoId == tecnicoId)
            .ExecuteDeleteAsync() > 0;
    }

    public async Task<List<Tecnicos>> Listar(Expression<Func<Tecnicos, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Tecnicos
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }
}
