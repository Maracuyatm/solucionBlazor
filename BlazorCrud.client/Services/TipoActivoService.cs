using BlazorCrud.Shared;
using System.Net.Http.Json;

namespace BlazorCrud.Client.Services
{
    public class TipoActivoService : ITipoActivoService
    {
        private readonly HttpClient _httpClient;

        public TipoActivoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<TipoActivoDTO>> Lista()
        {
            var result = await _httpClient.GetFromJsonAsync<ResponseAPI<List<TipoActivoDTO>>>("api/TipoActivo/Lista");
            if (result!.EsCorrecto) return result.Valor!;
            throw new Exception(result.Mensaje);
        }
    }
}