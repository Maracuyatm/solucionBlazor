using BlazorCrud.Shared;
using System.Net.Http.Json;

namespace BlazorCrud.client.Services
{
    public class EmpleadoService : IEmpleadoService
    {
        private readonly HttpClient _httpClient;

        public EmpleadoService(HttpClient http)
        {
            _httpClient = http;
        }
        public async Task<List<EmpleadoDTO>> Lista()
        {
            
            var result = await _httpClient.GetFromJsonAsync<ResponseAPI<List<EmpleadoDTO>>>("api/Empleado/Lista");
            if (result!.EsCorrecto) return result.Valor;
            else
                throw new Exception(result.Mensaje);
        }

        public async Task<EmpleadoDTO> Buscar(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<ResponseAPI<EmpleadoDTO>>($"api/Empleado/Buscar/{id}");
            if (result!.EsCorrecto) return result.Valor;
            else
                throw new Exception(result.Mensaje);
        }

        public async Task<int> Guardar(EmpleadoDTO empleado)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/Empleado/Guardar/", empleado);
            var response = await result.Content.ReadFromJsonAsync<ResponseAPI<int>>();
            if (response!.EsCorrecto) return response.Valor;
            else
                throw new Exception(response.Mensaje);
        }

        public async Task<int> Editar(EmpleadoDTO empleado)
        {
            var result = await _httpClient.PutAsJsonAsync($"api/Empleado/Editar/{empleado.IdEmpleado}", empleado);
            var response = await result.Content.ReadFromJsonAsync<ResponseAPI<int>>();
            if (response!.EsCorrecto) return response.Valor;
            else
                throw new Exception(response.Mensaje);
        }

        public async Task<bool> Eliminar(int id)
        {
            var result = await _httpClient.DeleteAsync($"api/Empleado/Eliminar/{id}");
            var response = await result.Content.ReadFromJsonAsync<ResponseAPI<int>>();
            if (response!.EsCorrecto) return response.EsCorrecto;
            else
                throw new Exception(response.Mensaje);
        }

        

        
    }
}
