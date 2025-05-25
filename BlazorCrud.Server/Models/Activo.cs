using System;

namespace BlazorCrud.Server.Models
{
    public partial class Activo
    {
        public Guid Id { get; set; }  // Clave primaria GUID

        public Guid TipoActivoId { get; set; }  // FK a tipo_activo
        public Guid MarcaId { get; set; }       // FK a marca
        public Guid EstadoId { get; set; }      // FK a estado

        public string Serial { get; set; } = null!;  // Número de serie

        public Guid? SistemaOperativoId { get; set; }  // FK a sistema_operativo
        public string? Modelo { get; set; }            // Modelo del equipo
        public Guid? Procesador { get; set; }          // FK a procesador

        public string Descripcion { get; set; } = null!;  // Nuevo nombre del campo antes llamado "Nombre"

        public short? Numero { get; set; }          // Número interno o código
        public short? Ram { get; set; }             // RAM en GB
        public short? Almacenamiento { get; set; }  // Almacenamiento en GB

        public DateTime FechaAdquisicion { get; set; }  // Fecha de compra

        public DateTime? CreatedAt { get; set; }    // Fecha de creación
        public DateTime? UpdatedAt { get; set; }    // Fecha de modificación
        public DateTime? DeletedAt { get; set; }    // Soft delete
    }
}
