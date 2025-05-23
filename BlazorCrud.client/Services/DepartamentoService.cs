using BlazorCrud.Shared;
using System.Net.Http.Json;

namespace BlazorCrud.client.Services
{
    public class DepartamentoService : IDepartamentoService
    {
        private readonly HttpClient _httpClient;
        public DepartamentoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<DepartamentoDTO>> Lista()
        {
            var result = await _httpClient.GetFromJsonAsync<ResponseAPI<List<DepartamentoDTO>>>("api/Departamento/Lista");
            if (result!.EsCorrecto) return result.Valor;
            else
                throw new Exception(result.Mensaje);
        }
    }
}
