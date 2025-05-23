using BlazorCrud.Shared;

namespace BlazorCrud.Client.Services
{
    public interface IProcesadorService
    {
        Task<List<ProcesadorDTO>> Lista();
        Task<ProcesadorDTO> Buscar(Guid id);
        Task<ResponseAPI<Guid>> Guardar(ProcesadorDTO procesador);

        Task<ResponseAPI<Guid>> Editar(ProcesadorDTO procesador, Guid id);

        Task Activar(Guid id);

        Task<bool> Desactivar(Guid id);

        Task<bool> Eliminar(Guid id);
    }
}
