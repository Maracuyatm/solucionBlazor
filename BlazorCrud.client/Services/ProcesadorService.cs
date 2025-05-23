using BlazorCrud.Shared;
using System.Net.Http.Json;

namespace BlazorCrud.Client.Services
{
    public class ProcesadorService : IProcesadorService
    {
        private readonly HttpClient _httpClient;

        public ProcesadorService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProcesadorDTO>> Lista()
        {
            var result = await _httpClient.GetFromJsonAsync<ResponseAPI<List<ProcesadorDTO>>>("api/Procesador/Lista");
            if (result!.EsCorrecto) return result.Valor!;
            throw new Exception(result.Mensaje);
        }

        public async Task<ProcesadorDTO> Buscar(Guid id)
        {
            var result = await _httpClient.GetFromJsonAsync<ResponseAPI<ProcesadorDTO>>($"api/Procesador/Buscar/{id}");
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
        public async Task<ResponseAPI<Guid>> Guardar(ProcesadorDTO procesador)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Procesador/Guardar", procesador);

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
        public async Task<ResponseAPI<Guid>> Editar(ProcesadorDTO procesador, Guid id)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Procesador/editar/{id}", procesador);

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
            var response = await _httpClient.PostAsync($"api/Procesador/Activar/{id}", null);
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException("No se pudo activar el procesador");
            }
        }


        public async Task<bool> Desactivar(Guid id)
        {
            var response = await _httpClient.PutAsync($"api/Procesador/Desactivar/{id}", null);
            var result = await response.Content.ReadFromJsonAsync<ResponseAPI<bool>>();

            if (result!.EsCorrecto) return result.Valor;
            throw new Exception(result.Mensaje);
        }


        public async Task<bool> Eliminar(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/Procesador/Eliminar/{id}");
            var result = await response.Content.ReadFromJsonAsync<ResponseAPI<Guid>>();

            if (result!.EsCorrecto) return true;
            throw new Exception(result.Mensaje);
        }
    }
}
