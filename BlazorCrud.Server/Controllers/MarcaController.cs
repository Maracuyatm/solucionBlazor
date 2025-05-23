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
            var listaMarcaDTO = new List<MarcaDTO>();

            try
            {
                var marcas = await _dbContext.Marca
                    .Where(m => m.DeletedAt == null)
                    .OrderByDescending(m => m.CreatedAt)
                    .ToListAsync();

                foreach (var item in marcas)
                {
                    listaMarcaDTO.Add(new MarcaDTO
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
                responseApi.Valor = listaMarcaDTO;
            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }

            return Ok(responseApi);
        }


        // Buscar marca por ID
        [HttpGet("Buscar/{id}")]
        public async Task<IActionResult> Buscar(Guid id)
        {
            var responseApi = new ResponseAPI<MarcaDTO>();

            try
            {
                var dbMarca = await _dbContext.Marca.FirstOrDefaultAsync(m => m.Id == id);

                if (dbMarca != null)
                {
                    responseApi.EsCorrecto = true;
                    responseApi.Valor = new MarcaDTO
                    {
                        Id = dbMarca.Id,
                        Nombre = dbMarca.Nombre,
                        CreatedAt = dbMarca.CreatedAt,
                        UpdatedAt = dbMarca.UpdatedAt,
                        DeletedAt = dbMarca.DeletedAt
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


        // Guardar una nueva marca
        [HttpPost("Guardar")]
        public async Task<IActionResult> Guardar(MarcaDTO marcaDTO)
        {
            var responseApi = new ResponseAPI<Guid>();

            try
            {
                // Validar que no exista una marca con el mismo nombre (ignorando mayúsculas/minúsculas)
                bool existeNombre = await _dbContext.Marca
                    .AnyAsync(m => m.Nombre.ToLower() == marcaDTO.Nombre.ToLower() && m.DeletedAt == null);

                if (existeNombre)
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Ya existe una marca con ese nombre.";
                    return BadRequest(responseApi);
                }

                var dbMarca = new Marca
                {
                    Id = Guid.NewGuid(),
                    Nombre = marcaDTO.Nombre,
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
                var dbMarca = await _dbContext.Marca.FirstOrDefaultAsync(m => m.Id == id);

                if (dbMarca == null)
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Marca no encontrada";
                    return NotFound(responseApi);
                }

                // Verificar si ya existe otra marca con el mismo nombre
                bool nombreDuplicado = await _dbContext.Marca
                    .AnyAsync(m => m.Id != id &&
                                   m.Nombre.ToLower() == marcaDTO.Nombre.ToLower() &&
                                   m.DeletedAt == null);

                if (nombreDuplicado)
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Ya existe otra marca con ese nombre.";
                    return BadRequest(responseApi);
                }

                dbMarca.Nombre = marcaDTO.Nombre;
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
            var marca = await _dbContext.Marca.FindAsync(id);
            if (marca == null)
                return NotFound();

            marca.Estado = 1; // o el valor que indique activo
            _dbContext.Marca.Update(marca);
            await _dbContext.SaveChangesAsync();

            return NoContent();
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
                if (marca == null)
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Marca no encontrada";
                    return NotFound(responseApi);
                }

                // Realizar el Soft Delete: solo actualiza el campo DeletedAt
                marca.DeletedAt = DateTime.Now;
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
