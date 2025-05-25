using BlazorCrud.Shared;

namespace BlazorCrud.Client.Services
{
    public interface IMarcaService
    {
        Task<List<MarcaDTO>> Lista();
        Task<MarcaDTO> Buscar(Guid id);
        Task<ResponseAPI<Guid>> Guardar(MarcaDTO marca);

        Task<ResponseAPI<Guid>> Editar(MarcaDTO marca, Guid id);

        Task Activar(Guid id);

        Task<bool> Desactivar(Guid id);

        Task<bool> ExisteMarcaConNombreYTipoAsync(string nombre, Guid tipoActivoId, Guid? idMarcaExcluir = null);


        Task<bool> Eliminar(Guid id);
    }
}
