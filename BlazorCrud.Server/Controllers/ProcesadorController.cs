using BlazorCrud.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using BlazorCrud.Shared;

namespace BlazorCrud.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcesadorController : ControllerBase
    {
        private readonly DbcrudBlazorContext _dbContext;

        public ProcesadorController(DbcrudBlazorContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("Lista")]
        public async Task<IActionResult> Lista()
        {
            var responseApi = new ResponseAPI<List<ProcesadorDTO>>();
            var listaProcesadorDTO = new List<ProcesadorDTO>();

            try
            {
                var procesadores = await _dbContext.Procesador
                    .Where(m => m.DeletedAt == null)
                    .OrderByDescending(m => m.CreatedAt)
                    .ToListAsync();

                foreach (var item in procesadores)
                {
                    listaProcesadorDTO.Add(new ProcesadorDTO
                    {
                        Id = item.Id,
                        Nombre = item.Nombre,
                        CreatedAt = item.CreatedAt,
                        UpdatedAt = item.UpdatedAt,
                        DeletedAt = item.DeletedAt,
                        Estado = item.Estado
                    });
                }

                responseApi.EsCorrecto = true;
                responseApi.Valor = listaProcesadorDTO;
            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }

            return Ok(responseApi);
        }

        [HttpGet("Buscar/{id}")]
        public async Task<IActionResult> Buscar(Guid id)
        {
            var responseApi = new ResponseAPI<ProcesadorDTO>();

            try
            {
                var dbProcesador = await _dbContext.Procesador.FirstOrDefaultAsync(m => m.Id == id);

                if (dbProcesador != null)
                {
                    responseApi.EsCorrecto = true;
                    responseApi.Valor = new ProcesadorDTO
                    {
                        Id = dbProcesador.Id,
                        Nombre = dbProcesador.Nombre,
                        CreatedAt = dbProcesador.CreatedAt,
                        UpdatedAt = dbProcesador.UpdatedAt,
                        DeletedAt = dbProcesador.DeletedAt
                    };
                }
                else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Procesador no encontrada";
                }
            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }

            return Ok(responseApi);
        }

        [HttpPost("Guardar")]
        public async Task<IActionResult> Guardar(ProcesadorDTO procesadorDTO)
        {
            var responseApi = new ResponseAPI<Guid>();

            try
            {
                
                bool existeNombre = await _dbContext.Procesador
                    .AnyAsync(m => m.Nombre.ToLower() == procesadorDTO.Nombre.ToLower() && m.DeletedAt == null);

                if (existeNombre)
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Ya existe un procesador con ese nombre.";
                    return BadRequest(responseApi);
                }

                var dbProcesador = new Procesador
                {
                    Id = Guid.NewGuid(),
                    Nombre = procesadorDTO.Nombre,
                    Estado = 1,
                    CreatedAt = DateTime.UtcNow
                };

                _dbContext.Procesador.Add(dbProcesador);
                await _dbContext.SaveChangesAsync();

                responseApi.EsCorrecto = true;
                responseApi.Valor = dbProcesador.Id;
            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }

            return Ok(responseApi);
        }

        [HttpPut("Editar/{id}")]
        public async Task<IActionResult> Editar(ProcesadorDTO procesadorDTO, Guid id)
        {
            var responseApi = new ResponseAPI<Guid>();

            try
            {
                var dbProcesador = await _dbContext.Procesador.FirstOrDefaultAsync(m => m.Id == id);

                if (dbProcesador == null)
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Procesador no encontrada";
                    return NotFound(responseApi);
                }

                
                bool nombreDuplicado = await _dbContext.Procesador
                    .AnyAsync(m => m.Id != id &&
                                   m.Nombre.ToLower() == procesadorDTO.Nombre.ToLower() &&
                                   m.DeletedAt == null);

                if (nombreDuplicado)
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Ya existe un procesador con ese nombre.";
                    return BadRequest(responseApi);
                }

                dbProcesador.Nombre = procesadorDTO.Nombre;
                dbProcesador.UpdatedAt = DateTime.UtcNow;

                _dbContext.Procesador.Update(dbProcesador);
                await _dbContext.SaveChangesAsync();

                responseApi.EsCorrecto = true;
                responseApi.Valor = dbProcesador.Id;
            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }

            return Ok(responseApi);
        }

        [HttpPost("Activar/{id}")]
        public async Task<IActionResult> Activar(Guid id)
        {
            var procesador = await _dbContext.Procesador.FindAsync(id);
            if (procesador == null)
                return NotFound();

            procesador.Estado = 1; // o el valor que indique activo
            _dbContext.Procesador.Update(procesador);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("Desactivar/{id}")]
        public async Task<IActionResult> Desactivar(Guid id)
        {
            var responseApi = new ResponseAPI<bool>();

            try
            {
                var procesador = await _dbContext.Procesador.FindAsync(id);

                if (procesador == null || procesador.DeletedAt != null)
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Procesador no encontrado.";
                    return NotFound(responseApi);
                }

                procesador.Estado = 0;
                procesador.UpdatedAt = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();

                responseApi.EsCorrecto = true;
                responseApi.Valor = true;
            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }

            return Ok(responseApi);
        }

        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Eliminar(Guid id)
        {
            var responseApi = new ResponseAPI<Guid>();

            try
            {
                var procesador = await _dbContext.Procesador.FirstOrDefaultAsync(m => m.Id == id);
                if (procesador == null)
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Procesador no encontrada";
                    return NotFound(responseApi);
                }

                // Realizar el Soft Delete: solo actualiza el campo DeletedAt
                procesador.DeletedAt = DateTime.Now;
                _dbContext.Procesador.Update(procesador);
                await _dbContext.SaveChangesAsync();

                responseApi.EsCorrecto = true;
                responseApi.Valor = id;
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
