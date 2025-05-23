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
            if (result!.EsCorrecto) return result.Valor!;
            throw new Exception(result.Mensaje);
        }

        public async Task<MarcaDTO> Buscar(Guid id)
        {
            var result = await _httpClient.GetFromJsonAsync<ResponseAPI<MarcaDTO>>($"api/Marca/Buscar/{id}");
            if (result!.EsCorrecto) return result.Valor!;
            throw new Exception(result.Mensaje);
        }

        //public async Task<Guid> Guardar(MarcaDTO marca)
        //{
        //    var response = await _httpClient.PostAsJsonAsync("api/Marca/Guardar", marca);
        //    var result = await response.Content.ReadFromJsonAsync<ResponseAPI<Guid>>();

        //    if (result!.EsCorrecto) return result.Valor!;
        //    throw new Exception(result.Mensaje);
        //}
        public async Task<ResponseAPI<Guid>> Guardar(MarcaDTO marca)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Marca/Guardar", marca);

            var result = await response.Content.ReadFromJsonAsync<ResponseAPI<Guid>>();

            // Manejar posibles errores HTTP (como 400 Bad Request)
            if (!response.IsSuccessStatusCode && result == null)
            {
                return new ResponseAPI<Guid>
                {
                    EsCorrecto = false,
                    Mensaje = "Error al comunicarse con el servidor."
                };
            }

            return result!;
        }


        //public async Task<Guid> Editar(MarcaDTO marca, Guid id)
        //{
        //    var response = await _httpClient.PutAsJsonAsync($"api/Marca/Editar/{id}", marca);
        //    var result = await response.Content.ReadFromJsonAsync<ResponseAPI<Guid>>();

        //    if (result!.EsCorrecto) return result.Valor!;
        //    throw new Exception(result.Mensaje);
        //}
        public async Task<ResponseAPI<Guid>> Editar(MarcaDTO marca, Guid id)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Marca/editar/{id}", marca);

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadFromJsonAsync<ResponseAPI<Guid>>();
                return resultado!;
            }
            else
            {
                var error = await response.Content.ReadFromJsonAsync<ResponseAPI<Guid>>();
                return error ?? new ResponseAPI<Guid> { EsCorrecto = false, Mensaje = "Error inesperado" };
            }
        }


        public async Task Activar(Guid id)
        {
            var response = await _httpClient.PostAsync($"api/Marca/Activar/{id}", null);
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException("No se pudo activar la marca");
            }
        }


        public async Task<bool> Desactivar(Guid id)
        {
            var response = await _httpClient.PutAsync($"api/Marca/Desactivar/{id}", null);
            var result = await response.Content.ReadFromJsonAsync<ResponseAPI<bool>>();

            if (result!.EsCorrecto) return result.Valor;
            throw new Exception(result.Mensaje);
        }


        public async Task<bool> Eliminar(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/Marca/Eliminar/{id}");
            var result = await response.Content.ReadFromJsonAsync<ResponseAPI<Guid>>();

            if (result!.EsCorrecto) return true;
            throw new Exception(result.Mensaje);
        }
    }
}
