using RegistroTecnicos.Models;
using RegistroTecnicos.DAL;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace RegistroTecnicos.Services;

public class ClienteService(IDbContextFactory<Contexto> DbFactory)
{
    public async Task<bool> Guardar(Clientes cliente)
    {
        if (!await Existe(cliente.Nombres, cliente.ClienteId))
        {
            return await Insertar(cliente);
        }
        else
        {
            return await Modificar(cliente);
        }
    }

    public async Task<bool> Existe(string nombres, int clienteId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Clientes
            .AnyAsync(c => c.Nombres.ToLower() == nombres.ToLower() && c.ClienteId != clienteId);
    }

    private async Task<bool> Insertar(Clientes cliente)
    {
        if (!await EsRncUnicoAsync(cliente.Rnc))
        {
            return false; // RNC ya existe
        }

        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Clientes.Add(cliente);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Clientes cliente)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Update(cliente);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Clientes?> Buscar(int clienteId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Clientes.FirstOrDefaultAsync(c => c.ClienteId == clienteId);
    }

    public async Task<bool> Eliminar(int clienteId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Clientes.AsNoTracking()
            .Where(c => c.ClienteId == clienteId)
            .ExecuteDeleteAsync() > 0;
    }

    public async Task<List<Clientes>> Listar(Expression<Func<Clientes, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Clientes
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> EsRncUnicoAsync(string rnc)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return !await contexto.Clientes.AnyAsync(c => c.Rnc == rnc);
    }

    public async Task<bool> GuardarClienteAsync(Clientes cliente)
    {
        if (await EsRncUnicoAsync(cliente.Rnc))
        {
            return await Insertar(cliente);
        }
        return false; // RNC ya existe
    }
}

