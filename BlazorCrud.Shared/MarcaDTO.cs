﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCrud.Shared
{
    public class MarcaDTO
    {
        public Guid Id { get; set; }
        public Guid TipoActivoId { get; set; }
        public string Tipo { get; set; } = string.Empty; // ← para mostrar el nombre del tipo

        [RegularExpression(@".*[A-Za-z].*", ErrorMessage = "El campo no puede ser sólo números")]
        [Required(ErrorMessage = "El nombre es requerido")]
        [MinLength(3, ErrorMessage = "El campo solicita al menos 3 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        [Range(0, 1, ErrorMessage = "El estado debe ser 0 (Inactivo) o 1 (Activo)")]
        public short Estado { get; set; } = 1; // Por defecto Activo
    }
}
