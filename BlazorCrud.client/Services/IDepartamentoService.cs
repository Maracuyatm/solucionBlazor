using BlazorCrud.Shared;

namespace BlazorCrud.client.Services
{
    public interface IDepartamentoService
    {
        Task<List<DepartamentoDTO>> Lista();
    }
}
