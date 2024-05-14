using DockerApi.Models;

namespace DockerApi.Services.Interface;

public interface IClienteService
{
    public Task<List<Cliente>> ClientListAsync();
    public Task<Cliente> GetClienteByIdAsync(int id);
    public Task<bool> AddClienteAsync(Cliente cliente);
    public Task<bool> UpdateClienteAsync(Cliente cliente);
    public Task<bool> DeleteClienteAsync(int id);
}