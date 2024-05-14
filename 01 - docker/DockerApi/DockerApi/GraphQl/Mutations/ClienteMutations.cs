using DockerApi.Models;
using DockerApi.Services.Interface;

namespace DockerApi.GraphQl.Mutations;

public class ClienteMutations
{
    public async Task<bool> AddClienteAsync([Service] IClienteService clienteService, Cliente cliente)
    {
        return await clienteService.AddClienteAsync(cliente);
    }

    public async Task<bool> UpdateClienteAsync([Service] IClienteService clienteService, Cliente cliente)
    {
        return await clienteService.UpdateClienteAsync(cliente);
    }

    public async Task<bool> DeleteClienteAsync([Service] IClienteService clienteService, int id)
    {
        return await clienteService.DeleteClienteAsync(id);
    }
}