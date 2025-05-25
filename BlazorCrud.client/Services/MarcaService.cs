using BlazorCrud.Shared;
using System.Net.Http.Json;

namespace BlazorCrud.Client.Services
{
    public class MarcaService : IMarcaService
    {
        private readonly HttpClient _httpClient;

        public MarcaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<MarcaDTO>> Lista()
        {
            var result = await _httpClient.GetFromJsonAsync<ResponseAPI<List<MarcaDTO>>>("api/Marca/Lista");
            if (result is { EsCorrecto: true, Valor: not null }) return result.Valor;
            throw new Exception(result?.Mensaje ?? "Error al obtener la lista de marcas.");
        }

        public async Task<MarcaDTO> Buscar(Guid id)
        {
            var result = await _httpClient.GetFromJsonAsync<ResponseAPI<MarcaDTO>>($"api/Marca/Buscar/{id}");
            if (result is { EsCorrecto: true, Valor: not null }) return result.Valor;
            throw new Exception(result?.Mensaje ?? "Error al buscar la marca.");
        }

        public async Task<ResponseAPI<Guid>> Guardar(MarcaDTO marca)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Marca/Guardar", marca);
            var result = await response.Content.ReadFromJsonAsync<ResponseAPI<Guid>>();

            return result ?? new ResponseAPI<Guid>
            {
                EsCorrecto = false,
                Mensaje = "No se pudo guardar la marca. Respuesta inesperada del servidor."
            };
        }

        public async Task<ResponseAPI<Guid>> Editar(MarcaDTO marca, Guid id)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Marca/Editar/{id}", marca);
            var result = await response.Content.ReadFromJsonAsync<ResponseAPI<Guid>>();

            return result ?? new ResponseAPI<Guid>
            {
                EsCorrecto = false,
                Mensaje = "No se pudo editar la marca. Respuesta inesperada del servidor."
            };
        }

        public async Task Activar(Guid id)
        {
            var response = await _httpClient.PostAsync($"api/Marca/Activar/{id}", null);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ResponseAPI<string>>();
                throw new Exception(error?.Mensaje ?? "No se pudo activar la marca.");
            }
        }

        public async Task<bool> Desactivar(Guid id)
        {
            var response = await _httpClient.PutAsync($"api/Marca/Desactivar/{id}", null);
            var result = await response.Content.ReadFromJsonAsync<ResponseAPI<bool>>();

            if (result is { EsCorrecto: true }) return result.Valor;
            throw new Exception(result?.Mensaje ?? "No se pudo desactivar la marca.");
        }

        public async Task<bool> Eliminar(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/Marca/Eliminar/{id}");
            var result = await response.Content.ReadFromJsonAsync<ResponseAPI<Guid>>();

            if (result is { EsCorrecto: true }) return true;
            throw new Exception(result?.Mensaje ?? "No se pudo eliminar la marca.");
        }

        

    }
}
