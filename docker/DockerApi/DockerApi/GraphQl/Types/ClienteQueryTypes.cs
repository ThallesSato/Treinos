using DockerApi.Models;
using DockerApi.Services.Interface;

namespace DockerApi.GraphQl.Types;

public class ClienteQueryTypes
{
    public async Task<List<Cliente>> GetClienteListAsync([Service] IClienteService clienteService)
    {
        return await clienteService.ClientListAsync();
    }

    public async Task<Cliente> GetClienteByIdAsync([Service] IClienteService clienteService, int id)
    {
        return await clienteService.GetClienteByIdAsync(id);
    }
}