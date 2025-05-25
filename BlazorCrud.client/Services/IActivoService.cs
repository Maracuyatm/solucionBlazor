using BlazorCrud.Shared;

namespace BlazorCrud.Client.Services
{
    public interface IActivoService
    {
        Task<List<ActivoDTO>> Lista();
        Task<ActivoDTO> Buscar(Guid id);
        Task<ResponseAPI<Guid>> Guardar(ActivoDTO activo);
        Task<ResponseAPI<Guid>> Editar(ActivoDTO activo, Guid id);
        //Task<ResponseAPI<bool>> Activar(Guid id);
        //Task<ResponseAPI<bool>> Desactivar(Guid id);
        Task<bool> Eliminar(Guid id);
    }
}
