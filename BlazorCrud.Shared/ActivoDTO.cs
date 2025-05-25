using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorCrud.Shared
{
    public class ActivoDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El tipo de activo es requerido")]
        public Guid TipoActivoId { get; set; }

        [Required(ErrorMessage = "La marca es requerida")]
        public Guid MarcaId { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        public Guid EstadoId { get; set; }

        [Required(ErrorMessage = "El número de serie es requerido")]
        [StringLength(50, ErrorMessage = "El serial no puede superar los 50 caracteres")]
        public string Serial { get; set; } = string.Empty;

        public Guid? SistemaOperativoId { get; set; }

        [StringLength(50, ErrorMessage = "El modelo no puede superar los 50 caracteres")]
        public string? Modelo { get; set; }

        public Guid? Procesador { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(250, ErrorMessage = "El nombre no puede superar los 250 caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        public short? Numero { get; set; }

        public short? Ram { get; set; }

        public short? Almacenamiento { get; set; }

        [Required(ErrorMessage = "La fecha de adquisición es requerida")]
        public DateTime FechaAdquisicion { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
