using DockerApi.Database;
using DockerApi.Models;
using DockerApi.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace DockerApi.Services;

public class ClienteService : IClienteService
{
    private readonly AppDbContext _appDbContext;

    public ClienteService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<List<Cliente>> ClientListAsync()
    {
        return await _appDbContext.Clientes.ToListAsync();
    }

    public async Task<Cliente> GetClienteByIdAsync(int id)
    {
        return await _appDbContext.Clientes.Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<bool> AddClienteAsync(Cliente cliente)
    {
        await _appDbContext.Clientes.AddAsync(cliente);
        var result = await _appDbContext.SaveChangesAsync();
        if (result <= 0) return false;
        return true;
    }

    public async Task<bool> UpdateClienteAsync(Cliente cliente)
    {
        if (!ClienteExists(cliente.Id)) return false;
        
        _appDbContext.Clientes.Update(cliente);
        var result = await _appDbContext.SaveChangesAsync();
        if (result <= 0) return false;
        return true;
    }

    public async Task<bool> DeleteClienteAsync(int id)
    {
        var cliente = _appDbContext.Clientes.FirstOrDefault(x => x.Id == id);
        if (cliente == null) return false;

        _appDbContext.Clientes.Remove(cliente);
        var result = await _appDbContext.SaveChangesAsync();
        if (result <= 0) return false;
        return true;
    }

    private bool ClienteExists(int id)
    {
        return _appDbContext.Clientes.Any(x => x.Id == id);
    }
}