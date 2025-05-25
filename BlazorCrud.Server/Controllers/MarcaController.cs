using BlazorCrud.Server.Models;
using BlazorCrud.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

namespace BlazorCrud.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarcaController : ControllerBase
    {
        private readonly DbcrudBlazorContext _dbContext;

        public MarcaController(DbcrudBlazorContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Obtener lista de marcas
        [HttpGet("Lista")]
        public async Task<IActionResult> Lista()
        {
            var responseApi = new ResponseAPI<List<MarcaDTO>>();

            try
            {
                var marcas = await _dbContext.Marca
                    .Where(m => m.DeletedAt == null)
                    .Include(m => m.TipoActivo)
                    .OrderByDescending(m => m.CreatedAt)
                    .Select(m => new MarcaDTO
                    {
                        Id = m.Id,
                        Nombre = m.Nombre,
                        TipoActivoId = m.TipoActivoId,
                        Tipo = m.TipoActivo.Nombre,
                        Estado = m.Estado,
                        CreatedAt = m.CreatedAt,
                        UpdatedAt = m.UpdatedAt,
                        DeletedAt = m.DeletedAt
                    })
                    .ToListAsync();

                responseApi.EsCorrecto = true;
                responseApi.Valor = marcas;
            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }

            return Ok(responseApi);
        }

        //Buscar marca
        [HttpGet("Buscar/{id}")]
        public async Task<IActionResult> Buscar(Guid id)
        {
            var responseApi = new ResponseAPI<MarcaDTO>();

            try
            {
                var dbMarca = await _dbContext.Marca
                    .Include(m => m.TipoActivo) // ← join con TipoActivo
                    .FirstOrDefaultAsync(m => m.Id == id && m.DeletedAt == null); // soft delete

                if (dbMarca != null)
                {
                    responseApi.EsCorrecto = true;
                    responseApi.Valor = new MarcaDTO
                    {
                        Id = dbMarca.Id,
                        Nombre = dbMarca.Nombre,
                        TipoActivoId = dbMarca.TipoActivoId,
                        Tipo = dbMarca.TipoActivo?.Nombre ?? string.Empty, // ← por si es null
                        CreatedAt = dbMarca.CreatedAt,
                        UpdatedAt = dbMarca.UpdatedAt,
                        DeletedAt = dbMarca.DeletedAt,
                        Estado = dbMarca.Estado
                    };
                }
                else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Marca no encontrada";
                }
            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }

            return Ok(responseApi);
        }


        //Guardar marca
        [HttpPost("Guardar")]
        public async Task<IActionResult> Guardar(MarcaDTO marcaDTO)
        {
            var responseApi = new ResponseAPI<Guid>();

            try
            {
                // Validación del modelo (opcional, si usas atributos [Required])
                if (!ModelState.IsValid)
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Datos inválidos.";
                    return BadRequest(responseApi);
                }

                // Verifica si ya existe una marca con el mismo nombre (insensible a mayúsculas)
                bool existeNombre = await _dbContext.Marca
                    .AnyAsync(m => m.Nombre.ToUpper() == marcaDTO.Nombre.ToUpper() && m.DeletedAt == null);

                if (existeNombre)
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Ya existe una marca con ese nombre.";
                    return BadRequest(responseApi);
                }

                var dbMarca = new Marca
                {
                    Id = Guid.NewGuid(),
                    Nombre = marcaDTO.Nombre.Trim(),
                    TipoActivoId = marcaDTO.TipoActivoId,
                    Estado = 1,
                    CreatedAt = DateTime.UtcNow
                };

                _dbContext.Marca.Add(dbMarca);
                await _dbContext.SaveChangesAsync();

                responseApi.EsCorrecto = true;
                responseApi.Valor = dbMarca.Id;
            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }

            return Ok(responseApi);
        }



        // Editar una marca existente
        [HttpPut("Editar/{id}")]
        public async Task<IActionResult> Editar(MarcaDTO marcaDTO, Guid id)
        {
            var responseApi = new ResponseAPI<Guid>();

            try
            {
                var dbMarca = await _dbContext.Marca
                    .FirstOrDefaultAsync(m => m.Id == id && m.DeletedAt == null);

                if (dbMarca == null)
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Marca no encontrada";
                    return NotFound(responseApi);
                }

                bool nombreDuplicado = await _dbContext.Marca
                    .AnyAsync(m => m.Id != id &&
                                   m.Nombre.ToUpper() == marcaDTO.Nombre.ToUpper() &&
                                   m.DeletedAt == null);

                if (nombreDuplicado)
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Ya existe otra marca con ese nombre.";
                    return BadRequest(responseApi);
                }

                dbMarca.Nombre = marcaDTO.Nombre.Trim();
                dbMarca.Estado = marcaDTO.Estado;
                dbMarca.UpdatedAt = DateTime.UtcNow;

                _dbContext.Marca.Update(dbMarca);
                await _dbContext.SaveChangesAsync();

                responseApi.EsCorrecto = true;
                responseApi.Valor = dbMarca.Id;
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
            var responseApi = new ResponseAPI<bool>();

            try
            {
                var marca = await _dbContext.Marca.FindAsync(id);

                if (marca == null || marca.DeletedAt != null)
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Marca no encontrada.";
                    return NotFound(responseApi);
                }

                marca.Estado = 1;
                marca.UpdatedAt = DateTime.UtcNow;

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



        [HttpPut("Desactivar/{id}")]
        public async Task<IActionResult> Desactivar(Guid id)
        {
            var responseApi = new ResponseAPI<bool>();

            try
            {
                var marca = await _dbContext.Marca.FindAsync(id);

                if (marca == null || marca.DeletedAt != null)
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Marca no encontrada.";
                    return NotFound(responseApi);
                }

                marca.Estado = 0;
                marca.UpdatedAt = DateTime.UtcNow;

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


        // Marcar como eliminado (soft delete)
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Eliminar(Guid id)
        {
            var responseApi = new ResponseAPI<Guid>();

            try
            {
                var marca = await _dbContext.Marca.FirstOrDefaultAsync(m => m.Id == id);

                if (marca == null || marca.DeletedAt != null)
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Marca no encontrada o ya eliminada.";
                    return NotFound(responseApi);
                }

                marca.DeletedAt = DateTime.UtcNow;
                marca.UpdatedAt = DateTime.UtcNow;

                _dbContext.Marca.Update(marca);
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
