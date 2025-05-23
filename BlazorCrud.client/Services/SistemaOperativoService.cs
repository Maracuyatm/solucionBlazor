using BlazorCrud.Shared;
using System.Net.Http.Json;

namespace BlazorCrud.Client.Services
{
    public class SistemaOperativoService : ISistemaOperativoService
    {
        private readonly HttpClient _httpClient;

        public SistemaOperativoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<SistemaOperativoDTO>> Lista()
        {
            var result = await _httpClient.GetFromJsonAsync<ResponseAPI<List<SistemaOperativoDTO>>>("api/SistemaOperativo/Lista");
            if (result!.EsCorrecto) return result.Valor!;
            throw new Exception(result.Mensaje);
        }

        public async Task<SistemaOperativoDTO> Buscar(Guid id)
        {
            var result = await _httpClient.GetFromJsonAsync<ResponseAPI<SistemaOperativoDTO>>($"api/SistemaOperativo/Buscar/{id}");
            if (result!.EsCorrecto) return result.Valor!;
            throw new Exception(result.Mensaje);
        }

        //public async Task<Guid> Guardar(SistemaOperativoDTO sistemaOperativo)
        //{
        //    var response = await _httpClient.PostAsJsonAsync("api/SistemaOperativo/Guardar", sistemaOperativo);
        //    var result = await response.Content.ReadFromJsonAsync<ResponseAPI<Guid>>();

        //    if (result!.EsCorrecto) return result.Valor!;
        //    throw new Exception(result.Mensaje);
        //}
        public async Task<ResponseAPI<Guid>> Guardar(SistemaOperativoDTO sistemaOperativo)
        {
            var response = await _httpClient.PostAsJsonAsync("api/SistemaOperativo/Guardar", sistemaOperativo);

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


        //public async Task<Guid> Editar(SistemaOperativoDTO sistemaOperativo, Guid id)
        //{
        //    var response = await _httpClient.PutAsJsonAsync($"api/SistemaOperativo/Editar/{id}", sistemaOperativo);
        //    var result = await response.Content.ReadFromJsonAsync<ResponseAPI<Guid>>();

        //    if (result!.EsCorrecto) return result.Valor!;
        //    throw new Exception(result.Mensaje);
        //}
        public async Task<ResponseAPI<Guid>> Editar(SistemaOperativoDTO sistemaOperativo, Guid id)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/SistemaOperativo/editar/{id}", sistemaOperativo);

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
            var response = await _httpClient.PostAsync($"api/SistemaOperativo/Activar/{id}", null);
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException("No se pudo activar la sistemaOperativo");
            }
        }


        public async Task<bool> Desactivar(Guid id)
        {
            var response = await _httpClient.PutAsync($"api/SistemaOperativo/Desactivar/{id}", null);
            var result = await response.Content.ReadFromJsonAsync<ResponseAPI<bool>>();

            if (result!.EsCorrecto) return result.Valor;
            throw new Exception(result.Mensaje);
        }


        public async Task<bool> Eliminar(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/SistemaOperativo/Eliminar/{id}");
            var result = await response.Content.ReadFromJsonAsync<ResponseAPI<Guid>>();

            if (result!.EsCorrecto) return true;
            throw new Exception(result.Mensaje);
        }
    }
}
