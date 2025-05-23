using BlazorCrud.Shared;

namespace BlazorCrud.Client.Services
{
    public interface ISistemaOperativoService
    {
        Task<List<SistemaOperativoDTO>> Lista();
        Task<SistemaOperativoDTO> Buscar(Guid id);
        Task<ResponseAPI<Guid>> Guardar(SistemaOperativoDTO sistemaOperativo);

        Task<ResponseAPI<Guid>> Editar(SistemaOperativoDTO sistemaOperativo, Guid id);

        Task Activar(Guid id);

        Task<bool> Desactivar(Guid id);

        Task<bool> Eliminar(Guid id);
    }
}
