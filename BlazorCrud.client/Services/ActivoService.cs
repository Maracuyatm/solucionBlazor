using BlazorCrud.Shared;
using System.Net.Http.Json;

namespace BlazorCrud.Client.Services
{
    public class ActivoService : IActivoService
    {
        private readonly HttpClient _httpClient;

        public ActivoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ActivoDTO>> Lista()
        {
            var result = await _httpClient.GetFromJsonAsync<ResponseAPI<List<ActivoDTO>>>("api/Activo/Lista");
            if (result!.EsCorrecto) return result.Valor!;
            throw new Exception(result.Mensaje);
        }

        public async Task<ActivoDTO> Buscar(Guid id)
        {
            var result = await _httpClient.GetFromJsonAsync<ResponseAPI<ActivoDTO>>($"api/Activo/Buscar/{id}");
            if (result!.EsCorrecto) return result.Valor!;
            throw new Exception(result.Mensaje);
        }

        public async Task<ResponseAPI<Guid>> Guardar(ActivoDTO activo)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Activo/Guardar", activo);
            var result = await response.Content.ReadFromJsonAsync<ResponseAPI<Guid>>();

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

        public async Task<ResponseAPI<Guid>> Editar(ActivoDTO activo, Guid id)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Activo/Editar/{id}", activo);

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

        //public async Task<ResponseAPI<bool>> Activar(Guid id)
        //{
        //    var response = await _httpClient.PostAsync($"api/Activo/Activar/{id}", null);
        //    var result = await response.Content.ReadFromJsonAsync<ResponseAPI<bool>>();

        //    if (result == null || !result.EsCorrecto)
        //    {
        //        return result ?? new ResponseAPI<bool>
        //        {
        //            EsCorrecto = false,
        //            Mensaje = "Error al activar el activo"
        //        };
        //    }

        //    return result;
        //}

        //public async Task<ResponseAPI<bool>> Desactivar(Guid id)
        //{
        //    var response = await _httpClient.PutAsync($"api/Activo/Desactivar/{id}", null);
        //    var result = await response.Content.ReadFromJsonAsync<ResponseAPI<bool>>();

        //    if (result == null || !result.EsCorrecto)
        //    {
        //        return result ?? new ResponseAPI<bool>
        //        {
        //            EsCorrecto = false,
        //            Mensaje = "Error al desactivar el activo"
        //        };
        //    }

        //    return result;
        //}

        public async Task<bool> Eliminar(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/Activo/Eliminar/{id}");
            var result = await response.Content.ReadFromJsonAsync<ResponseAPI<Guid>>();

            if (result!.EsCorrecto) return true;
            throw new Exception(result.Mensaje);
        }
    }
}
