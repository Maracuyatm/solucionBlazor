using BlazorCrud.Server.Models;
using BlazorCrud.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

namespace BlazorCrud.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SistemaOperativoController : ControllerBase
    {
        private readonly DbcrudBlazorContext _dbContext;

        public SistemaOperativoController(DbcrudBlazorContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Obtener lista de sistemaOperativo
        [HttpGet("Lista")]
        public async Task<IActionResult> Lista()
        {
            var responseApi = new ResponseAPI<List<SistemaOperativoDTO>>();
            var listaSistemaOperativoDTO = new List<SistemaOperativoDTO>();

            try
            {
                var sistemaOperativo = await _dbContext.SistemaOperativo
                    .Where(m => m.DeletedAt == null)
                    .OrderByDescending(m => m.CreatedAt)
                    .ToListAsync();

                foreach (var item in sistemaOperativo)
                {
                    listaSistemaOperativoDTO.Add(new SistemaOperativoDTO
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
                responseApi.Valor = listaSistemaOperativoDTO;
            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }

            return Ok(responseApi);
        }


        // Buscar sistemaOperativo por ID
        [HttpGet("Buscar/{id}")]
        public async Task<IActionResult> Buscar(Guid id)
        {
            var responseApi = new ResponseAPI<SistemaOperativoDTO>();

            try
            {
                var dbSistemaOperativo = await _dbContext.SistemaOperativo.FirstOrDefaultAsync(m => m.Id == id);

                if (dbSistemaOperativo != null)
                {
                    responseApi.EsCorrecto = true;
                    responseApi.Valor = new SistemaOperativoDTO
                    {
                        Id = dbSistemaOperativo.Id,
                        Nombre = dbSistemaOperativo.Nombre,
                        CreatedAt = dbSistemaOperativo.CreatedAt,
                        UpdatedAt = dbSistemaOperativo.UpdatedAt,
                        DeletedAt = dbSistemaOperativo.DeletedAt
                    };
                }
                else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Sistema operativo no encontrado";
                }
            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }

            return Ok(responseApi);
        }


        // Guardar una nueva sistemaOperativo
        [HttpPost("Guardar")]
        public async Task<IActionResult> Guardar(SistemaOperativoDTO sistemaOperativoDTO)
        {
            var responseApi = new ResponseAPI<Guid>();

            try
            {
                // Validar que no exista una sistemaOperativo con el mismo nombre (ignorando mayúsculas/minúsculas)
                bool existeNombre = await _dbContext.SistemaOperativo
                    .AnyAsync(m => m.Nombre.ToLower() == sistemaOperativoDTO.Nombre.ToLower() && m.DeletedAt == null);

                if (existeNombre)
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Ya existe un sistema operativo con ese nombre.";
                    return BadRequest(responseApi);
                }

                var dbSistemaOperativo = new SistemaOperativo
                {
                    Id = Guid.NewGuid(),
                    Nombre = sistemaOperativoDTO.Nombre,
                    Estado = 1,
                    CreatedAt = DateTime.UtcNow
                };

                _dbContext.SistemaOperativo.Add(dbSistemaOperativo);
                await _dbContext.SaveChangesAsync();

                responseApi.EsCorrecto = true;
                responseApi.Valor = dbSistemaOperativo.Id;
            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }

            return Ok(responseApi);
        }


        // Editar una sistemaOperativo existente
        [HttpPut("Editar/{id}")]
        public async Task<IActionResult> Editar(SistemaOperativoDTO sistemaOperativoDTO, Guid id)
        {
            var responseApi = new ResponseAPI<Guid>();

            try
            {
                var dbSistemaOperativo = await _dbContext.SistemaOperativo.FirstOrDefaultAsync(m => m.Id == id);

                if (dbSistemaOperativo == null)
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Sistema operativo no encontrado";
                    return NotFound(responseApi);
                }

                // Verificar si ya existe otra sistemaOperativo con el mismo nombre
                bool nombreDuplicado = await _dbContext.SistemaOperativo
                    .AnyAsync(m => m.Id != id &&
                                   m.Nombre.ToLower() == sistemaOperativoDTO.Nombre.ToLower() &&
                                   m.DeletedAt == null);

                if (nombreDuplicado)
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Ya existe otro sistema operativo con ese nombre.";
                    return BadRequest(responseApi);
                }

                dbSistemaOperativo.Nombre = sistemaOperativoDTO.Nombre;
                dbSistemaOperativo.UpdatedAt = DateTime.UtcNow;

                _dbContext.SistemaOperativo.Update(dbSistemaOperativo);
                await _dbContext.SaveChangesAsync();

                responseApi.EsCorrecto = true;
                responseApi.Valor = dbSistemaOperativo.Id;
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
            var sistemaOperativo = await _dbContext.SistemaOperativo.FindAsync(id);
            if (sistemaOperativo == null)
                return NotFound();

            sistemaOperativo.Estado = 1; // o el valor que indique activo
            _dbContext.SistemaOperativo.Update(sistemaOperativo);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }


        [HttpPut("Desactivar/{id}")]
        public async Task<IActionResult> Desactivar(Guid id)
        {
            var responseApi = new ResponseAPI<bool>();

            try
            {
                var sistemaOperativo = await _dbContext.SistemaOperativo.FindAsync(id);

                if (sistemaOperativo == null || sistemaOperativo.DeletedAt != null)
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Sistema operativo no encontrado.";
                    return NotFound(responseApi);
                }

                sistemaOperativo.Estado = 0;
                sistemaOperativo.UpdatedAt = DateTime.UtcNow;
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


        // SistemaOperativor como eliminado (soft delete)
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Eliminar(Guid id)
        {
            var responseApi = new ResponseAPI<Guid>();

            try
            {
                var sistemaOperativo = await _dbContext.SistemaOperativo.FirstOrDefaultAsync(m => m.Id == id);
                if (sistemaOperativo == null)
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Sistema operativo no encontrado";
                    return NotFound(responseApi);
                }

                // Realizar el Soft Delete: solo actualiza el campo DeletedAt
                sistemaOperativo.DeletedAt = DateTime.Now;
                _dbContext.SistemaOperativo.Update(sistemaOperativo);
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
