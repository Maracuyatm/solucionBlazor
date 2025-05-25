using BlazorCrud.Server.Models;
using BlazorCrud.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorCrud.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivoController : ControllerBase
    {
        private readonly DbcrudBlazorContext _dbContext;

        public ActivoController(DbcrudBlazorContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Obtener lista de activos
        [HttpGet("Lista")]
        public async Task<IActionResult> Lista()
        {
            var responseApi = new ResponseAPI<List<ActivoDTO>>();
            var lista = new List<ActivoDTO>();

            try
            {
                var activos = await _dbContext.Activo
                    .Where(a => a.DeletedAt == null)
                    .OrderByDescending(a => a.CreatedAt)
                    .ToListAsync();

                foreach (var a in activos)
                {
                    lista.Add(new ActivoDTO
                    {
                        Id = a.Id,
                        TipoActivoId = a.TipoActivoId,
                        MarcaId = a.MarcaId,
                        EstadoId = a.EstadoId,
                        Serial = a.Serial,
                        SistemaOperativoId = a.SistemaOperativoId,
                        Modelo = a.Modelo,
                        Procesador = a.Procesador,
                        Descripcion = a.Descripcion,
                        Numero = a.Numero,
                        Ram = a.Ram,
                        Almacenamiento = a.Almacenamiento,
                        FechaAdquisicion = a.FechaAdquisicion,
                        CreatedAt = a.CreatedAt,
                        UpdatedAt = a.UpdatedAt,
                        DeletedAt = a.DeletedAt
                    });
                }

                responseApi.EsCorrecto = true;
                responseApi.Valor = lista;
            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }

            return Ok(responseApi);
        }

        // Buscar activo por ID
        [HttpGet("Buscar/{id}")]
        public async Task<IActionResult> Buscar(Guid id)
        {
            var responseApi = new ResponseAPI<ActivoDTO>();

            try
            {
                var activo = await _dbContext.Activo.FirstOrDefaultAsync(a => a.Id == id);

                if (activo != null)
                {
                    responseApi.EsCorrecto = true;
                    responseApi.Valor = new ActivoDTO
                    {
                        Id = activo.Id,
                        TipoActivoId = activo.TipoActivoId,
                        MarcaId = activo.MarcaId,
                        EstadoId = activo.EstadoId,
                        Serial = activo.Serial,
                        SistemaOperativoId = activo.SistemaOperativoId,
                        Modelo = activo.Modelo,
                        Procesador = activo.Procesador,
                        Descripcion = activo.Descripcion,
                        Numero = activo.Numero,
                        Ram = activo.Ram,
                        Almacenamiento = activo.Almacenamiento,
                        FechaAdquisicion = activo.FechaAdquisicion,
                        CreatedAt = activo.CreatedAt,
                        UpdatedAt = activo.UpdatedAt,
                        DeletedAt = activo.DeletedAt
                    };
                }
                else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Activo no encontrado";
                }
            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }

            return Ok(responseApi);
        }

        // Guardar nuevo activo
        [HttpPost("Guardar")]
        public async Task<IActionResult> Guardar(ActivoDTO dto)
        {
            var responseApi = new ResponseAPI<Guid>();

            try
            {
                var activo = new Activo
                {
                    Id = Guid.NewGuid(),
                    TipoActivoId = dto.TipoActivoId,
                    MarcaId = dto.MarcaId,
                    EstadoId = dto.EstadoId,
                    Serial = dto.Serial,
                    SistemaOperativoId = dto.SistemaOperativoId,
                    Modelo = dto.Modelo,
                    Procesador = dto.Procesador,
                    Descripcion = dto.Descripcion,
                    Numero = dto.Numero,
                    Ram = dto.Ram,
                    Almacenamiento = dto.Almacenamiento,
                    FechaAdquisicion = dto.FechaAdquisicion,
                    CreatedAt = DateTime.UtcNow
                };

                _dbContext.Activo.Add(activo);
                await _dbContext.SaveChangesAsync();

                responseApi.EsCorrecto = true;
                responseApi.Valor = activo.Id;
            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }

            return Ok(responseApi);
        }

        // Editar un activo existente
        [HttpPut("Editar/{id}")]
        public async Task<IActionResult> Editar(ActivoDTO dto, Guid id)
        {
            var responseApi = new ResponseAPI<Guid>();

            try
            {
                var dbActivo = await _dbContext.Activo.FirstOrDefaultAsync(a => a.Id == id);

                if (dbActivo == null)
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Activo no encontrado";
                    return NotFound(responseApi);
                }

                dbActivo.TipoActivoId = dto.TipoActivoId;
                dbActivo.MarcaId = dto.MarcaId;
                dbActivo.EstadoId = dto.EstadoId;
                dbActivo.Serial = dto.Serial;
                dbActivo.SistemaOperativoId = dto.SistemaOperativoId;
                dbActivo.Modelo = dto.Modelo;
                dbActivo.Procesador = dto.Procesador;
                dbActivo.Descripcion = dto.Descripcion;
                dbActivo.Numero = dto.Numero;
                dbActivo.Ram = dto.Ram;
                dbActivo.Almacenamiento = dto.Almacenamiento;
                dbActivo.FechaAdquisicion = dto.FechaAdquisicion;
                dbActivo.UpdatedAt = DateTime.UtcNow;

                _dbContext.Activo.Update(dbActivo);
                await _dbContext.SaveChangesAsync();

                responseApi.EsCorrecto = true;
                responseApi.Valor = dbActivo.Id;
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
