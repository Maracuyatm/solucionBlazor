using BlazorCrud.Server.Models;
using BlazorCrud.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorCrud.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoActivoController : ControllerBase
    {
        private readonly DbcrudBlazorContext _dbContext;

        public TipoActivoController(DbcrudBlazorContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Obtener lista de tipos de activo
        [HttpGet("Lista")]
        public async Task<IActionResult> Lista()
        {
            var responseApi = new ResponseAPI<List<TipoActivoDTO>>();
            var listaTipoActivoDTO = new List<TipoActivoDTO>();

            try
            {
                var tiposActivo = await _dbContext.TipoActivo
                    .Where(t => t.DeletedAt == null)
                    .OrderByDescending(t => t.CreatedAt)
                    .ToListAsync();

                foreach (var item in tiposActivo)
                {
                    listaTipoActivoDTO.Add(new TipoActivoDTO
                    {
                        Id = item.Id,
                        Nombre = item.Nombre,
                        CreatedAt = item.CreatedAt,
                        UpdatedAt = item.UpdatedAt,
                        DeletedAt = item.DeletedAt,
                    });
                }

                responseApi.EsCorrecto = true;
                responseApi.Valor = listaTipoActivoDTO;
            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }

            return Ok(responseApi);
        }
    }
}
